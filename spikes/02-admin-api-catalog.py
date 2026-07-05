"""
Spike 2: Admin API endpoint catalog from HAR
=================================================================
Extract and categorize all API endpoints found in the HAR log.
The admin panel (`link.freshliance.com`) uses a different API than
the Partner API (`api.freshliance.com`) documented in the spec.
"""

import json

# ═══════════════════════════════════════════════════════════════════
# Endpoints extracted from HAR (XHR calls only)
# ═══════════════════════════════════════════════════════════════════

ADMIN_ENDPOINTS = [
    {
        "method": "GET",
        "path": "/admin-api/system/dict-data/list-all-simple",
        "description": "List all dictionary data (enums, dropdowns)",
        "params": ["lang", "timeZone"],
        "auth": "Cookie session",
    },
    {
        "method": "GET",
        "path": "/admin-api/gw/user/profile/get",
        "description": "Get current user profile (admin panel user, NOT public API user)",
        "params": ["lang", "timeZone"],
        "auth": "Cookie session",
    },
    {
        "method": "GET",
        "path": "/admin-api/system/auth/get-permission-info",
        "description": "Get permissions for current user (menus, roles)",
        "params": ["lang", "timeZone"],
        "auth": "Cookie session",
    },
    {
        "method": "POST",
        "path": "/admin-api/gw/product/info/get/device/model",
        "description": "Get device product models (categories)",
        "params": ["lang", "timeZone"],
        "body": "{}",
        "auth": "Cookie session",
    },
    {
        "method": "GET",
        "path": "/admin-api/gw/floor/page",
        "description": "Get floor plans page",
        "params": ["pageNo=1", "pageSize=20", "planName=", "lang", "timeZone"],
        "auth": "Cookie session",
    },
    {
        "method": "GET",
        "path": "/admin-api/gw/push-record/getPushRecordCount",
        "description": "Get push notification record count",
        "params": ["lang", "timeZone"],
        "auth": "Cookie session",
    },
    {
        "method": "GET",
        "path": "/admin-api/gw/user/device/allDevicePage",
        "description": "Get all user devices (paginated) — full device dashboard list",
        "params": ["pageNo=1", "pageSize=30", "deviceStatus=", "productType=",
                    "recordStatus=", "deviceSn=", "deviceName=", "powerStatus=",
                    "lang", "timeZone"],
        "auth": "Cookie session",
    },
]

# ═══════════════════════════════════════════════════════════════════
# Key observations from HAR
# ═══════════════════════════════════════════════════════════════════

OBSERVATIONS = {
    "admin_vs_partner": """
    +---------------------+--------------------------------+-----------------------------------+
    |                     | Admin API (link.freshliance)  | Partner API (api.freshliance)     |
    +---------------------+--------------------------------+-----------------------------------+
    | Base URL            | /admin-api/gw/...             | /api                              |
    | Auth                | Cookie session (login-based)  | RSA2 signature + appId            |
    | HTTP Methods        | Mix of GET + POST             | All POST                          |
    | Pagination param    | pageNo                        | pageNum                           |
    | i18n                | ?lang=en_US&timeZone=%2B05:00 | Accept-Language header            |
    | Enum/lookup data    | /system/dict-data             | N/A (not in partner API)          |
    | Permission model    | /system/auth/                 | N/A (appId-scoped)                |
    +---------------------+--------------------------------+-----------------------------------+
    """,

    "pagination_difference": """
    The public Partner API docs use "pageNum" as the parameter name.
    The Admin API uses "pageNo". This is a CRITICAL difference.
    
    Example:
      Partner:  bizContent={"pageNum":1,"pageSize":20}
      Admin:    ?pageNo=1&pageSize=20
    """,

    "i18n_difference": """
    Partner API: Accept-Language header (per docs Rule section)
    Admin API:   Query params ?lang=en_US&timeZone=%2B05:00
    
    Note: The Partner API docs mention Accept-Language but the Admin
    panel uses query params. Both approaches may work for Partner API.
    """,

    "common_patterns": """
    Both APIs share:
    - Same response envelope (code/msg/data/sign/subCode/subMsg)
    - Same domain concepts (devices, groups, records, alarms)
    - Same enum values (deviceStatus, productType, etc.)
    
    The Admin API has additional:
    - Dictionary/lookup endpoints
    - Permission/role management
    - Floor plan management
    - Push notification tracking
    """,

    "har_session": """
    HAR captured from: https://link.freshliance.com/plan/index
    Cookies: session-based (login required)
    Server: nginx/1.25.1, IP: 8.222.202.207 (Alibaba Cloud Singapore)
    OSS: AliyunOSS for static assets
    """,

    "missing_from_har": """
    Admin endpoints NOT captured in this HAR but likely exist
    (based on JS function names in the bundle):
    - gw/deviceGroup/treeList or similar (group management)
    - gw/deviceData/page or similar (device data)
    - gw/deviceCmd/... (remote configuration)
    - gw/configTemplate/... (sensor templates)
    These would be captured by navigating the admin panel further.
    """,
}

# ═══════════════════════════════════════════════════════════════════
# Admin API → Partner API mapping
# ═══════════════════════════════════════════════════════════════════

API_MAPPING = {
    "gw/user/profile/get": "gw.userInfo.get",  # Admin calls this; likely same underlying service
    "gw/product/info/get/device/model": "gw.device.category",  # Same concept
    "gw/user/device/allDevicePage": "gw.deviceInfo.page",  # Same concept, different param names
}


if __name__ == "__main__":
    print("=" * 60)
    print("SPIKE 2: Admin API endpoint catalog")
    print("=" * 60)

    print("\n## ADMIN API ENDPOINTS (from HAR)")
    print("-" * 40)
    for ep in ADMIN_ENDPOINTS:
        print(f"\n  {ep['method']} {ep['path']}")
        print(f"    {ep['description']}")
        print(f"    Auth: {ep['auth']}")

    print("\n\n## KEY OBSERVATIONS")
    print("-" * 40)
    for key, value in OBSERVATIONS.items():
        print(f"\n### {key}")
        print(value)

    print("\n\n## ADMIN → PARTNER API MAPPING")
    print("-" * 40)
    for admin, partner in API_MAPPING.items():
        print(f"  {admin}  →  {partner}")

    print("\n## SUMMARY")
    print(f"  Admin endpoints in HAR: {len(ADMIN_ENDPOINTS)}")
    print(f"  Partner endpoints in docs: 28")
    print(f"  Key risk: pagination param mismatch (pageNo vs pageNum)")
