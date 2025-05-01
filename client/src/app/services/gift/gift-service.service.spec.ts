import { TestBed } from '@angular/core/testing';

import { GiftServiceService } from './gift-service.service';

describe('GiftServiceService', () => {
  let service: GiftServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GiftServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
