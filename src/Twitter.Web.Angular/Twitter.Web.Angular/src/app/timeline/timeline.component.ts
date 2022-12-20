import { Component, OnInit, ViewChild } from '@angular/core';
import { TweetService } from '../services/tweet.service';
import { Tweet } from '../models/tweet';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-timeline',
  template: `
    <cdk-virtual-scroll-viewport
      class="timeline"
      (scrolledIndexChange)="onScroll()"
      [itemSize]="itemSize"
      [minBufferPx]="minBufferPx"
      [maxBufferPx]="maxBufferPx"
    >
      <div *cdkVirtualFor="let tweet of tweets" class="tweet">
        {{ tweet.text }}
      </div>
    </cdk-virtual-scroll-viewport>
  `,
  styleUrls: ['./timeline.component.scss']
})
export class TimelineComponent implements OnInit {
  tweets: Tweet[] = [];
  loading = false;
  itemSize = 50;
  minBufferPx = 100;
  maxBufferPx = 200;

  @ViewChild(CdkVirtualScrollViewport)
  viewport!: CdkVirtualScrollViewport;

  constructor(private tweetService: TweetService) {}

  ngOnInit() {
    this.refreshTimeline();
  }

  async refreshTimeline() {
    this.loading = true;
    this.tweets = await this.tweetService.getTimeline().toPromise() ?? [];
    this.loading = false;
  }

  async onScroll() {
    if (this.loading) {
      return;
    }
    const viewport = this.viewport;
    const end = viewport.getRenderedRange().end;
    const total = viewport.getDataLength();
    if (end === total) {
      this.loading = true;
      const newTweets = await this.tweetService.getTimeline().toPromise();
      this.tweets.push(...newTweets ?? []);
      this.loading = false;
    }
  }
}
