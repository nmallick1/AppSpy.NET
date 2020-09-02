import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeSelectorContainerComponent } from './code-selector-container.component';

describe('CodeSelectorContainerComponent', () => {
  let component: CodeSelectorContainerComponent;
  let fixture: ComponentFixture<CodeSelectorContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CodeSelectorContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeSelectorContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
