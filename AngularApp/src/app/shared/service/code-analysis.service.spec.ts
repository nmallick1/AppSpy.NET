import { TestBed } from '@angular/core/testing';

import { CodeAnalysisService } from './code-analysis.service';

describe('CodeAnalysisService', () => {
  let service: CodeAnalysisService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CodeAnalysisService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
