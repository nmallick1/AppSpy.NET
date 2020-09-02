import { TestBed } from '@angular/core/testing';

import { DecompilerService } from './decompiler.service';

describe('DecompilerService', () => {
  let service: DecompilerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DecompilerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
