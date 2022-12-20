import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TweetInputComponent } from './tweet-input.component';

describe('TweetInputComponent', () => {
  let component: TweetInputComponent;
  let fixture: ComponentFixture<TweetInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TweetInputComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TweetInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
