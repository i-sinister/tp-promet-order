export interface EditableOrderProperties {
  number: string,
  date: string,
  providerID: number,
}

export interface OrderInfo extends EditableOrderProperties {
  id: number,
  providerName: string,
}

export interface OrderListResponse {
  items: OrderInfo[],
  count: number,
}

export interface OrderDetails extends OrderInfo {
  items: OrderItem[],
}

export interface CreateOrderRequest extends EditableOrderProperties {
  items: CreateOrderItem[],
}

export interface UpdateOrderRequest extends EditableOrderProperties {
  items: UpdateOrderItem[],
}

export interface EditableOrderItemProperties {
  name: string,
  quantity: number,
  unit: string,
}

export interface OrderItem extends EditableOrderItemProperties {
  id: number,
  orderID: number,
}

export interface CreateOrderItem extends EditableOrderItemProperties {
}

export interface UpdateOrderItem extends EditableOrderItemProperties {
  id: number,
}

export interface ProviderInfo {
  id: number,
  name: string
}

export interface ProviderListResponse {
  items: ProviderInfo[],
}
