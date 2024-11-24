import http from 'k6/http';
import {sleep, check} from 'k6'; 

export const options={
    vus: 500,
    duration: '3m',
}
export default function(){
    http.get('http://localhost:6100/weather/trabzon?days=10');
    sleep(1);
    
}