import { Component, OnInit, ViewChild } from '@angular/core';
import { TweetService } from '../services/tweet.service';
import { Tweet } from '../models/tweet';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.scss']
})
export class TimelineComponent implements OnInit {
  tweets: Tweet[] | undefined;
  loading = false;
  itemSize = 50;
  minBufferPx = 100;
  maxBufferPx = 200;

  @ViewChild(CdkVirtualScrollViewport)
  viewport!: CdkVirtualScrollViewport;

  constructor(private tweetService: TweetService) {}

  ngOnInit() {
    this.refreshTimeline();
    setInterval(this.onScroll, 5000);
  }

  async refreshTimeline() {
    this.loading = true;
    this.tweets = await this.tweetService.getTimeline().toPromise();
    console.log('this tweets', this.tweets);
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
      if(this.tweets == undefined) this.tweets = [];
      this.tweets.push(...newTweets ?? []);
      this.loading = false;
    }
  }
}
