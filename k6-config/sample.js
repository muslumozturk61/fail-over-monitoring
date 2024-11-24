import http from "k6/http";
import {sleep, check} from 'k6'; 

/*export let options = {
  vus: 5,
  duration: "30s",
  thresholds: {
    http_req_duration: ["p(95)<500"], //tüm isteklerin uçtan uca süresi (yani toplam gecikme süresi)
  },
};*/

export default function () {
    var domain = 'https://test.k6.io'

    let res = http.get(domain,{tags: {name: '01_Home'}});
    check(res, {
    "is status 200": (r) => r.status === 200,
    'text verification': (r)=> r.body.includes("Collection of simple web-pages suitable for load testing")
  });
  sleep(1);

  res = http.get(domain + '/flip_coin.php',{
    tags: {name: '02_VisitFlipCoin'}});
    check(res, {
    "is status 200": (r) => r.status === 200,
    'text verification': (r)=> r.body.includes("Your Bet")
  });
 


  };