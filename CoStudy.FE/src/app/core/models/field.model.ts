export interface Field {
  oid: string;
  value: string;
  status: number;
  created_date?: string;
}

export interface FieldGroup {
  oid: string;
  group_name: string;
  fields: Field[];
  field_id?: string[];
  status: number;
  status_name: string;
  index?: number;
}

export interface AddFieldRequest {
  value: string;
}

export interface AddFieldGroupRequest {
  group_name: string;
  field_id: string[];
}

export interface AddFieldToGroupRequest {
  group_id: string;
  group_name: string;
  field_ids: string[];
}

export interface FieldApiResponse<T> {
  result: T;
}

export interface FieldPaginationRequest {
  skip?: number;
  count?: number;
}
