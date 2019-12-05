import { async, ComponentFixture, TestBed } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HeaderComponent } from './header.component';
import {UserProviderMock} from '../model/mocks';
import {UserProviderBase} from '../service/user-provider';
import {EventBus} from '../service/event-bus';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeaderComponent ],
      imports: [RouterTestingModule],
      providers: [
        { provide: UserProviderBase, useClass: UserProviderMock },
        EventBus
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
