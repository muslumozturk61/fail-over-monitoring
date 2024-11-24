import http from 'k6/http';

export default function () {
  http.get('http://localhost:6101/weather/trabzon?days=10');
}