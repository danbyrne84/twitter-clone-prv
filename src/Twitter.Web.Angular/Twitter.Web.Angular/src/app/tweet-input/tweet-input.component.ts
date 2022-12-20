import { Component, OnInit, Inject, Injectable } from '@angular/core';
import { TweetService } from '../services/tweet.service';
import { Tweet } from '../models/tweet';

@Injectable()
@Component({
  selector: 'app-tweet-input',
  templateUrl: './tweet-input.component.html',
  styleUrls: ['./tweet-input.component.scss']
})
export class TweetInputComponent implements OnInit {
  text = '';

  constructor(private tweetService: TweetService) {}

  ngOnInit() {}

  submitTweet() {
    this.tweetService.createTweet(this.text).subscribe();
    this.text = '';
  }
}