import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {}

  createUser(username: string, password: string) {
    return this.http.post(`${environment.baseUrl}/users`, { username, password });
  }

  getUser(userId: number) {
    return this.http.get(`${environment.baseUrl}/users/${userId}`);
  }

  getTimeline(userId: number, maxTweets?: number) {
    let url = `${environment.baseUrl}/users/${userId}/tweets`;
    if (maxTweets) {
      url += `?max_tweets=${maxTweets}`;
    }
    return this.http.get(url);
  }

  follow(userId: number, followId: number) {
    return this.http.post(`/users/${userId}/follow`, { followId });
  }

  unfollow(userId: number, followId: number) {
    return this.http.delete(`/users/${userId}/follow/${followId}`);
  }

  getFollowers(userId: number) {
    return this.http.get(`/users/${userId}/followers`);
  }

  getFollowing(userId: number) {
    return this.http.get(`/users/${userId}/following`);
  }
}
