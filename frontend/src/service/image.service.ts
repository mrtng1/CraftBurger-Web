// src/app/services/image.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor() { }

  getImageUrl(itemName: string, itemType: 'burger' | 'fries'): string {
    if (!itemName) {
      return '/path/to/default-image.jpg';
    }

    const formattedName = itemName.toLowerCase().replace(/\s+/g, '-');
    const folderPath = itemType === 'burger' ? 'burgers' : 'fries';
    const fileExtension = itemType === 'burger' ? 'jpg' : 'jpg';

    return `/assets/${folderPath}/${formattedName}.${fileExtension}`;
  }
}
