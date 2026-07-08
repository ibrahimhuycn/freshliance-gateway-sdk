using Freshliance.Dashboard.Components.Shared;
using FreshlianceGateway.Sdk.Models.Group;
using FreshlianceGateway.Sdk.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Freshliance.Dashboard.Components.Pages;

public partial class Groups
{
    [Inject] private IGroupService GroupService { get; set; } = null!;
    [Inject] private IGroupDeviceService GroupDeviceService { get; set; } = null!;
    [Inject] private IDeviceService DeviceService { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private List<TreeItemData<GroupTreeNodeResponse>>? _treeItems;
    private GroupTreeNodeResponse? _selectedNode;
    private string? _errorMessage;
    private bool _isLoading;
    private MudDataGrid<AllocatedDeviceResponse> _grid = null!;
    private int _totalAllocated;

    protected override async Task OnInitializedAsync()
    {
        await LoadTreeAsync();
    }

    private async Task LoadTreeAsync()
    {
        _isLoading = true;
        _errorMessage = null;
        try
        {
            var response = await GroupService.GetTreeAsync();
            response.EnsureSuccess();
            _treeItems = response.Data?.Select(ToTreeItemData).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private static TreeItemData<GroupTreeNodeResponse> ToTreeItemData(GroupTreeNodeResponse node)
    {
        return new TreeItemData<GroupTreeNodeResponse>
        {
            Value = node,
            Text = node.GroupName,
            Children = node.SubDeviceGroupList?.Select(ToTreeItemData).ToList()
        };
    }

    private async Task OnTreeItemSelected(GroupTreeNodeResponse? value)
    {
        _selectedNode = value;
        if (_grid is not null)
            await _grid.ReloadServerData();
    }

    private async Task OnRenameContext(GroupTreeNodeResponse node)
    {
        _selectedNode = node;
        if (_grid is not null)
            await _grid.ReloadServerData();
        await OpenRenameDialog(node);
    }

    private async Task OnDeleteContext(GroupTreeNodeResponse node)
    {
        _selectedNode = node;
        if (_grid is not null)
            await _grid.ReloadServerData();
        await DeleteGroup(node);
    }

    private async Task<GridData<AllocatedDeviceResponse>> LoadAllocatedDevices(GridState<AllocatedDeviceResponse> state, CancellationToken token)
    {
        if (_selectedNode is null)
            return new GridData<AllocatedDeviceResponse> { Items = [], TotalItems = 0 };

        try
        {
            var request = new GetAllocatedDeviceRequest
            {
                GroupId = _selectedNode.GroupId,
                PageNum = state.Page + 1,
                PageSize = state.PageSize
            };
            var response = await GroupDeviceService.GetAllocatedPageAsync(request);
            response.EnsureSuccess();
            _totalAllocated = response.Data?.Total ?? 0;
            return new GridData<AllocatedDeviceResponse>
            {
                Items = response.Data?.Rows ?? [],
                TotalItems = response.Data?.Total ?? 0
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load devices: {ex.Message}", Severity.Error);
            return new GridData<AllocatedDeviceResponse> { Items = [], TotalItems = 0 };
        }
    }

    private async Task OpenAddGroupDialog()
    {
        var parameters = new DialogParameters
        {
            ["ParentId"] = _selectedNode?.GroupId ?? 0
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AddGroupDialog>("Add Group", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled && result.Data is CreateGroupRequest request)
        {
            try
            {
                var response = await GroupService.CreateAsync(request);
                response.EnsureSuccess();
                await LoadTreeAsync();
                Snackbar.Add("Group created successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to create group: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OpenRenameDialog(GroupTreeNodeResponse? node = null)
    {
        var target = node ?? _selectedNode;
        if (target is null) return;

        var parameters = new DialogParameters
        {
            ["GroupId"] = target.GroupId,
            ["GroupName"] = target.GroupName
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<RenameGroupDialog>("Rename Group", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled && result.Data is UpdateGroupRequest request)
        {
            try
            {
                var response = await GroupService.UpdateAsync(request);
                response.EnsureSuccess();
                if (_selectedNode?.GroupId == target.GroupId)
                    _selectedNode = null;
                await LoadTreeAsync();
                Snackbar.Add("Group renamed successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to rename group: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task DeleteGroup(GroupTreeNodeResponse? node = null)
    {
        var target = node ?? _selectedNode;
        if (target is null) return;

        var isNotEmpty = (target.DeviceGroupCount?.DeviceCount ?? 0) > 0;
        if (isNotEmpty)
        {
            Snackbar.Add("Cannot delete a group that contains devices", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters
        {
            ["Title"] = "Delete Group",
            ["Message"] = $"Delete group '{target.GroupName}'? This action cannot be undone.",
            ["ConfirmText"] = "Delete"
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            try
            {
                var response = await GroupService.DeleteAsync(new DeleteGroupRequest { GroupId = target.GroupId });
                response.EnsureSuccess();
                if (_selectedNode?.GroupId == target.GroupId)
                    _selectedNode = null;
                await LoadTreeAsync();
                Snackbar.Add("Group deleted", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to delete: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OpenAddDevicesDialog()
    {
        if (_selectedNode is null) return;

        var parameters = new DialogParameters
        {
            ["GroupId"] = _selectedNode.GroupId
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AddDevicesDialog>("Add Devices", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled && result.Data is BindDeviceRequest request)
        {
            try
            {
                var response = await GroupDeviceService.BindAsync(request);
                response.EnsureSuccess();
                if (_grid is not null)
                    await _grid.ReloadServerData();
                await LoadTreeAsync();
                Snackbar.Add("Devices added to group", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to add devices: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task RemoveDevice(AllocatedDeviceResponse device)
    {
        var parameters = new DialogParameters
        {
            ["Title"] = "Remove Device",
            ["Message"] = $"Remove '{device.DeviceName}' from this group?"
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled && _selectedNode is not null)
        {
            try
            {
                var response = await GroupDeviceService.UnbindAsync(new UnbindDeviceRequest
                {
                    UserDeviceId = device.UserDeviceId,
                    GroupId = _selectedNode.GroupId
                });
                response.EnsureSuccess();
                if (_grid is not null)
                    await _grid.ReloadServerData();
                await LoadTreeAsync();
                Snackbar.Add("Device removed from group", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to remove device: {ex.Message}", Severity.Error);
            }
        }
    }
}
