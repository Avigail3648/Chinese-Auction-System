import { TestBed } from '@angular/core/testing';

import { BasketServiceService } from './basket-service.service';

describe('BasketServiceService', () => {
  let service: BasketServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BasketServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
