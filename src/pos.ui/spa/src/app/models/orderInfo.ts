export interface OrderInfo {
		id: number,
		number: string,
		date: Date,
		providerID: number,
		providerName: string,
}
export interface OrderListResponse {
  items: OrderInfo[],
  count: number
}
