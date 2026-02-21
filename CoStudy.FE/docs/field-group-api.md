# Field & FieldGroup API – Research Notes

## Overview

- **Field** = một lĩnh vực học tập (e.g. "Toán", "Lập trình")
- **FieldGroup** = nhóm các lĩnh vực (e.g. "Khoa học tự nhiên" gồm nhiều Field)
- Cả hai dùng chung `IFieldServices`

---

## FIELD APIs — `POST /api/field`

### 1. Tạo Field
```
POST /api/field
```
**Body:**
```json
{ "value": "string" }
```
**Response:** Field vừa tạo
```json
{ "result": { "value": "Toán", "status": 0, "oid": "..." } }
```
> Chỉ cần gửi `value`. `status`, `created_date`, `modified_date` được set tự động.

---

### 2. Lấy danh sách Field (có phân trang)
```
GET /api/field?skip=0&count=20
```
**Query params:** `skip` (int, optional), `count` (int, optional)

**Response:** Mảng Field đang Active
```json
{ "result": [ { "oid": "...", "value": "Toán", "status": 0 } ] }
```
> Chỉ trả về các Field có `status = Active (0)`.

---

### 3. Lấy Field theo FieldGroup
```
GET /api/field/group-id/{groupId}
```
**Response:** Mảng Field thuộc nhóm đó.

---

### 4. Xóa Field
```
DELETE /api/field/{id}
```
**Response:** `"Xóa thành công"` hoặc exception nếu thất bại.

---

### 5. Thêm Field vào Group (từ FieldController)
```
POST /api/field/add-to-group
```
> Trùng chức năng với `/api/fieldgroup/add-field`. Xem bên dưới.

---

## FIELDGROUP APIs — `/api/fieldgroup`

### 1. Tạo FieldGroup
```
POST /api/fieldgroup
```
**Body:**
```json
{
  "group_name": "string",
  "field_id": ["fieldId1", "fieldId2"]
}
```
**Response:** `FieldGroupViewModel`
```json
{
  "result": {
    "oid": "...",
    "group_name": "Khoa học tự nhiên",
    "fields": [ { "oid": "...", "value": "Toán" } ],
    "status": 0,
    "status_name": "Active"
  }
}
```

---

### 2. Lấy tất cả FieldGroup (có phân trang)
```
GET /api/fieldgroup/field-group?skip=0&count=20
```
**Response:** Mảng `FieldGroupViewModel`

---

### 3. Cập nhật Field trong Group (thay thế toàn bộ)
```
POST /api/fieldgroup/add-field
```
**Body:**
```json
{
  "group_id": "string",
  "group_name": "string",
  "field_ids": ["fieldId1", "fieldId2"]
}
```
> ⚠️ **Lưu ý:** Backend **xóa toàn bộ** `field_id` hiện có rồi set lại từ `field_ids`. Đây là **replace**, không phải append.
> - `group_name` cũng được cập nhật cùng lúc.
> - Chỉ thêm field_id hợp lệ (tồn tại trong DB).

**Response:** `FieldGroupViewModel` sau khi cập nhật.

---

### 4. Xóa Field khỏi Group
```
POST /api/fieldgroup/remove-field
```
**Body:** (cùng format với `add-field`)
```json
{
  "group_id": "string",
  "group_name": "string",
  "field_ids": ["fieldIdToRemove"]
}
```
> Xóa các `field_ids` được gửi lên khỏi danh sách của group.

**Response:** `FieldGroupViewModel` sau khi cập nhật.

---

## Data Models

### `Field` (entity / response item)
| JSON | Type | Mô tả |
|---|---|---|
| `oid` | string | MongoDB ObjectId |
| `value` | string | Tên lĩnh vực |
| `status` | int | 0=Active, 1=Offline, 2=Blocked... |
| `created_date` | datetime | Auto-set |

### `FieldGroupViewModel` (response)
| JSON | Type | Mô tả |
|---|---|---|
| `oid` | string | MongoDB ObjectId |
| `group_name` | string | Tên nhóm |
| `fields` | `Field[]` | Danh sách field đầy đủ (resolved) |
| `status` | int | Trạng thái |
| `status_name` | string | "Active" / "Offline" / ... |
| `index` | int? | Chỉ số thứ tự |

### `AddFieldToGroupRequest`
| JSON | Type | Mô tả |
|---|---|---|
| `group_id` | string | ID của FieldGroup |
| `group_name` | string | Tên mới của group |
| `field_ids` | string[] | Danh sách field ID |

---

## TypeScript Interfaces

```typescript
interface Field {
  oid: string;
  value: string;
  status: number;
  created_date?: string;
}

interface FieldGroupViewModel {
  oid: string;
  group_name: string;
  fields: Field[];
  status: number;
  status_name: string;
  index?: number;
}

interface AddFieldRequest {
  value: string;
}

interface AddFieldGroupRequest {
  group_name: string;
  field_id: string[];
}

interface AddFieldToGroupRequest {
  group_id: string;
  group_name: string;
  field_ids: string[];
}

interface BaseGetAllRequest {
  skip?: number;
  count?: number;
}
```

---

## Lưu ý quan trọng

1. **`add-field` là REPLACE, không phải APPEND** — gửi toàn bộ danh sách field mới mỗi lần
2. **`FieldGroupViewModel.fields`** là danh sách `Field` đầy đủ (resolved từ `field_id[]`), không phải chỉ là ID
3. **`GET /api/field`** chỉ trả về field có `status = Active`
4. **Không có `[Authorize]`** trên các controller này → tất cả endpoint đều public (không cần JWT)
5. Cả `FieldController` và `FieldGroupController` đều dùng chung `IFieldServices`
