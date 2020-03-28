export interface Purchase {
  id: number;
  userId: number;
  productId: number;
  productName: string;
  productDescription: string;
  productPrice: number;
  isCancelled: boolean;
  dateAdded: Date;
}
