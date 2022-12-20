import { CdkVirtualScrollViewport, ScrollingModule } from '@angular/cdk/scrolling';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TimelineComponent } from './timeline/timeline.component';
import { HttpClientModule } from '@angular/common/http';
import { TweetInputComponent } from './tweet-input/tweet-input.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    TimelineComponent,
    TweetInputComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ScrollingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [CdkVirtualScrollViewport],
  bootstrap: [AppComponent]
})
export class AppModule { }
