export interface MenuItem {
  id: number;
  name: string;
  price: number;
  type: string;
  description: string | null;
  image: File | null;
}
