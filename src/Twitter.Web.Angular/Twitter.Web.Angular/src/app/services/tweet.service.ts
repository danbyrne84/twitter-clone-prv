import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Tweet } from '../models/tweet';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TweetService {
  constructor(private http: HttpClient) {}

  getTimeline(maxTweets?: number): Observable<Tweet[]> {
    let url = `${environment.baseUrl}/tweets`;
    if (maxTweets) {
      url += `?max_tweets=${maxTweets}`;
    }
    return this.http.get<Tweet[]>(url);
  }

  createTweet(text: string): Observable<any> {
    return this.http.post(`${environment.baseUrl}/tweets`, { text });
  }

  getTweet(tweetId: number): Observable<Tweet> {
    return this.http.get<Tweet>(`${environment.baseUrl}/tweets/${tweetId}`);
  }
}
