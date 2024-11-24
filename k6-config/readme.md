

brew install k6
```

Users of Debian, Ubuntu or other Debian-based distros should run the following commands:
```bash
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 379CE192D401AB61
echo "deb https://dl.bintray.com/loadimpact/deb stable main" | sudo tee -a /etc/apt/sources.list
sudo apt-get update
sudo apt-get install k6
```

If you’re on Windows, you can [download the.msi installer](https://k6.io/docs/getting-started/installation/#windows-msi-installer). Users of other platforms, take a look at the [k6 installation guide](https://k6.io/docs/getting-started/installation/).

After the installation is complete, regardless of OS, you should be able to run {% raw %}`k6 version` to see the current version number and verify the installation was a success.

#### Writing and Running Your First Test Script

To perform k6 tests, you’ll need to create a test script. Test scripts in k6 are written in JavaScript, which is a perfect choice, considering most web developers will already be familiar with the language—as opposed to something like Python, Ruby, or a custom DSL.

Using your favorite editor, create a file called **script.js**. I’ll use Visual Studio Code:

```
code script.js
```

Then, add the following code to the new file:

```javascript
import http from 'k6/http';

export default function () {
  http.get('http://localhost:5000/Api/TodoItems');
}
```

Of course, make sure that you’ve started the API. Also, change the port number if necessary.

After saving the test script, you can test it by running:

```
k6 run script.js
```

The results should look something like this:
```
          /\      |‾‾| /‾‾/   /‾‾/   
     /\  /  \     |  |/  /   /  /    
    /  \/    \    |     (   /   ‾‾\  
   /          \   |  |\  \ |  (‾)  | 
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: script.js
     output: -

  scenarios: (100.00%) 1 scenario, 1 max VUs, 10m30s max duration (incl. graceful stop):
           * default: 1 iterations for each of 1 VUs (maxDuration: 10m0s, gracefulStop: 30s)


running (00m00.1s), 0/1 VUs, 1 complete and 0 interrupted iterations
default ✓ [ 100% ] 1 VUs  00m00.1s/10m0s  1/1 iters, 1 per VU

     data_received..................: 1.8 kB 33 kB/s
     data_sent......................: 816 B  15 kB/s
     http_req_blocked...............: avg=22ms    min=5ms     med=22ms    max=39ms    p(90)=35.6ms   p(95)=37.3ms 
     http_req_connecting............: avg=499.9µs min=0s      med=499.9µs max=999.8µs p(90)=899.82µs p(95)=949.8µs
     http_req_duration..............: avg=3.99ms  min=999.7µs med=3.99ms  max=6.99ms  p(90)=6.39ms   p(95)=6.69ms 
       { expected_response:true }...: avg=3.99ms  min=999.7µs med=3.99ms  max=6.99ms  p(90)=6.39ms   p(95)=6.69ms 
     http_req_failed................: 0.00%  ✓ 0 ✗ 2
     http_req_receiving.............: avg=0s      min=0s      med=0s      max=0s      p(90)=0s       p(95)=0s     
     http_req_sending...............: avg=0s      min=0s      med=0s      max=0s      p(90)=0s       p(95)=0s     
     http_req_tls_handshaking.......: avg=19ms    min=0s      med=19ms    max=38ms    p(90)=34.2ms   p(95)=36.1ms 
     http_req_waiting...............: avg=3.99ms  min=999.7µs med=3.99ms  max=6.99ms  p(90)=6.39ms   p(95)=6.69ms 
     http_reqs......................: 2      37.030248/s
     iteration_duration.............: avg=54ms    min=54ms    med=54ms    max=54ms    p(90)=54ms     p(95)=54ms   
     iterations.....................: 1      18.515124/s
```
#### Improving The Test

You’ve just written and run your first k6 test script. Congrats! However, we need to beef it up a little bit so it compares with the test we’ve done using wrk. Our k6 test is, at the moment, a simple request. It sends a get request to the specified URL and then waits for 1 second.

When running a test with k6, you can pass parameters to the test. Run the following command:

```
k6 --vus 400 --duration 30s run script.js
```

You can see we’ve used two parameters here.  The **--duration** option defines that the test should run for 30 seconds.

The **--vus** option stands for [virtual users](https://k6.io/docs/misc/glossary/#virtual-users), which are used to send the requests to the system under test.To understand the concept of VUs, think of them like parallel infinite loops. If you go back to our script, you'll see there is a `default` function. Scripts in k6 need to have that `default` function as a requirement. That's the entry point for the test script. The code inside such a function is what the VUs execute; it gets executed over and over again, for the whole duration of the tests.

Besides the `default` function, you can have additional code. However, code outside `default` is run only once per VU. You use that code for initialization duties, such as importing different modules or loading something from your filesystem.

That being said, let’s see what the result of the command’s execution looks like:

```
          /\      |‾‾| /‾‾/   /‾‾/   
     /\  /  \     |  |/  /   /  /    
    /  \/    \    |     (   /   ‾‾\  
   /          \   |  |\  \ |  (‾)  | 
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: script.js
     output: -

  scenarios: (100.00%) 1 scenario, 400 max VUs, 1m0s max duration (incl. graceful stop):
           * default: 400 looping VUs for 30s (gracefulStop: 30s)


running (0m00.9s), 400/400 VUs, 8 complete and 0 interrupted iterations
default   [   3% ] 400 VUs  00.9s/30s

...

running (0m30.0s), 000/400 VUs, 202254 complete and 0 interrupted iterations
default ✓ [ 100% ] 400 VUs  30s

     data_received..................: 60 MB  2.0 MB/s
     data_sent......................: 36 MB  1.2 MB/s
     http_req_blocked...............: avg=1.66ms   min=0s    med=0s      max=2.68s p(90)=0s      p(95)=0s     
     http_req_connecting............: avg=213.12µs min=0s    med=0s      max=1.32s p(90)=0s      p(95)=0s     
     http_req_duration..............: avg=27.65ms  min=0s    med=24ms    max=1.96s p(90)=40ms    p(95)=54.99ms
       { expected_response:true }...: avg=27.65ms  min=0s    med=24ms    max=1.96s p(90)=40ms    p(95)=54.99ms
     http_req_failed................: 0.00%  ✓ 0     ✗ 404508
     http_req_receiving.............: avg=703.91µs min=0s    med=0s      max=1.35s p(90)=0s      p(95)=999.1µs
     http_req_sending...............: avg=167.24µs min=0s    med=0s      max=1.39s p(90)=0s      p(95)=0s     
     http_req_tls_handshaking.......: avg=1.3ms    min=0s    med=0s      max=2.65s p(90)=0s      p(95)=0s     
     http_req_waiting...............: avg=26.77ms  min=0s    med=24ms    max=1.46s p(90)=39.99ms p(95)=51ms   
     http_reqs......................: 404508 13474.167948/s
     iteration_duration.............: avg=59.23ms  min=998µs med=49.99ms max=2.84s p(90)=78.99ms p(95)=98.99ms
     iterations.....................: 202254 6737.083974/s
     vus............................: 380    min=380 max=400 
     vus_max........................: 400    min=400 max=400 
```
What you see above is what the results look like for me after running the latest version of the command. As you can see, there’s a lot going on here. Let’s walk through some of the main pieces of information:

*   `data_received`: that’s the total amount of data received (109 MB) at a rate of 3.4 MB per second;
*   `http_req_duration`: information about the duration of the HTTP requests performed, including the average, median, minimum and maximum values;
*   `http_req_failed`: the rate of requests that failed;
*   `http_req_waiting`: time spent waiting for the server’s response.

To learn more about the metrics displayed in k6’s results, you can [refer to the documentation](https://k6.io/docs/using-k6/metrics/).

### Testing With a Higher Load

We’ll now perform a different test, sending a PUT request instead of a GET one. We’ll also use an additional option of k6 that allows us to simulate an increase in the load on our application.

For that, I’ll create a new test script and name it script2.js. Here’s what its code looks like:

```javascript
import http from 'k6/http';

const url = 'https://localhost:5001/api/TodoItems/';

export let options = {
  stages: [
    { duration: '15s', target: 100 },
    { duration: '30s', target: 500 },
    { duration: '15s', target: 0 },
  ],
};  

export default function () {

  const headers = { 'Accept': '*/*', 'Content-Type': 'application/json', 'Host': 'localhost:5001' };
  const data = {
      name: 'Send e-mail',
      isComplete: false      
  };

  http.put(url, JSON.stringify(data), { headers: headers });
}
```

As you can see, in the script above we’re sending a PUT request to our API, trying to add a new TodoItem resource. Before that, though, we create an `options` variable in which we define three stages for our test. In the first one, which lasts for 15 seconds, k6 will increase the number of VUs from 0 to 100. Then, it will ramp up to 500 for another 30seconds, before ramping down to 0 in the last 15 seconds.

Here’s what the results look like:

```

          /\      |‾‾| /‾‾/   /‾‾/   
     /\  /  \     |  |/  /   /  /    
    /  \/    \    |     (   /   ‾‾\  
   /          \   |  |\  \ |  (‾)  | 
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: script2.js
     output: -

  scenarios: (100.00%) 1 scenario, 500 max VUs, 1m30s max duration (incl. graceful stop):
           * default: Up to 500 looping VUs for 1m0s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


running (0m00.8s), 006/500 VUs, 6743 complete and 0 interrupted iterations
default   [   1% ] 006/500 VUs  0m00.8s/1m00.0s

...

running (1m00.0s), 000/500 VUs, 671850 complete and 0 interrupted iterations
default ✓ [ 100% ] 000/500 VUs  1m0s

     data_received..............: 32 MB   537 kB/s
     data_sent..................: 84 MB   1.4 MB/s
     http_req_blocked...........: avg=73.54µs min=0s med=0s      max=530.01ms p(90)=0s      p(95)=0s     
     http_req_connecting........: avg=16.56µs min=0s med=0s      max=212ms    p(90)=0s      p(95)=0s     
     http_req_duration..........: avg=19.56ms min=0s med=14ms    max=307.99ms p(90)=41.99ms p(95)=55ms   
     http_req_failed............: 100.00% ✓ 671850 ✗ 0    
     http_req_receiving.........: avg=34.6µs  min=0s med=0s      max=229.99ms p(90)=0s      p(95)=0s     
     http_req_sending...........: avg=85.33µs min=0s med=0s      max=157ms    p(90)=0s      p(95)=999µs  
     http_req_tls_handshaking...: avg=56.37µs min=0s med=0s      max=364ms    p(90)=0s      p(95)=0s     
     http_req_waiting...........: avg=19.44ms min=0s med=14ms    max=272.99ms p(90)=41ms    p(95)=54.99ms
     http_reqs..................: 671850  11196.657731/s
     iteration_duration.........: avg=20.07ms min=0s med=14.99ms max=632.99ms p(90)=42.99ms p(95)=56.99ms
     iterations.................: 671850  11196.657731/s
     vus........................: 1       min=1    max=500
     vus_max....................: 500     min=500  max=500
```

## Here's Where a Scriptable Tool Shines: Testing a User Scenario

Up until now, we’ve been testing using fairly simple scripts. You might be wondering what’s all the fuss about scriptable tools after all. Well, here’s the thing: a non-scriptable tool might be enough if you only need to perform basic requests.

However, there are scenarios in which a non-scriptable tool can really make a difference. One of those is where you need to test a user scenario. In other words, you might need to verify a real usage workflow in your app. You simply can’t do that with a non-scriptable tool.

Up until now, we've been testing an endpoint in isolation. Often, when monitoring your services, you might have found a REST API underperforming at a certain load level and want to simulate this behavior again. A non-scriptable tool is often enough for this type of testing.

Such a type of verification, however, isn’t the most realistic. Why? Well, users don’t behave like that. They don’t do things in a completely isolated way. In the real world, your application continuously responds to a flow of real-user interactions, e.g., visit a page, log in, list items, purchase some items, and so on.

Testing real-world scenarios allow you to validate critical business logic or the most frequent user flows. If you want to mimic this type of interaction, a scriptable tool makes this job possible and more manageable.

The following script is a simple example of how testing a user workflow could look like. Using k6, we hit the GET endpoint, retrieving the existing resources. We then get the id of the first object, and use that to send another get  request, obtaining that resource. After that, we set the value of the `completed` attribute and send a PUT request to update the resource on the server. Finally, we use a check to verify whether the response has the expected status code.

```javascript
import http from 'k6/http';
import { check,  sleep } from 'k6';

export let options = {
    vus: 30,
    duration: '40s'
}

export default function () {
  let url = 'https://localhost:5001/api/TodoItems';

  // getting all todo items
  let response = http.get(url);

  // parsing the response body
  let obj = JSON.parse(response.body);

  // retrieving the id from the first resource
  let idFirstItem = obj[0].id;

  // retrieving the resource with the selected id
  response = http.get(`${url}/${idFirstItem}`);
  let item = JSON.parse(response.body);

  // setting the item as complete
  item.complete = true;

  // updating the item
  const headers = { 'Content-Type': 'application/json' };
  let res = http.put(`${url}/${idFirstItem}`, JSON.stringify(item), { headers: headers });

  // checking the response has the 204 (no content) status code
  check(res, {
    'is status 204': (r) => r.status === 204,
  });

  // random think time between 0 and 5 seconds
  sleep(Math.random() * 5);
}
```

In the previous example, the test runs the same user flow continuously until the test duration ends. But with k6, and you can also simulate different user flows with varying load patterns to run simultaneously. This flexibility allows you to test your application more precisely.

For example, you could run 50 virtual users doing some actions and 100 virtual users doing something different for a different period while generating a constant request rate to one endpoint. 

```javascript
export const options = {

  scenarios: {
    scenario1: {
      executor: 'constant-vus',
      duration: "1m",
      vus: 50,
      exec: "userFlowA"
    },

    scenario2: {
      executor: 'ramping-vus',
      stages: [
        { duration: '1m', target: 100 },
        { duration: '30s', target: 0 },
      ],
      exec: "userFlowB"
    },

    scenario3: {
      executor: 'constant-arrival-rate',
      rate: 200,
      timeUnit: '1s',
      duration: '1m',
      preAllocatedVUs: 50,
      exec: "hammerEndpointA"
    }
  }
}
```

To learn more about configuring advanced traffic patterns, check out the [Scenarios API](https://k6.io/docs/using-k6/scenarios/).


## Here's Where k6 Shines: Setting Performance Goals

The ability to set performance goals for your application is an aspect that differentiates k6 from other load testing tools. With k6, you can verify the performance of your app against expected baselines in a way that’s [not that different from assertions in unit testing](https://k6.io/unit-testing-for-performance/).

How does that work in practice? There are two main options that k6 provides: checks and thresholds. 

[Checks ](https://k6.io/docs/using-k6/checks/) allow you to set expectations in your script and verify those expectations automatically. We’ve used one check in our previous example. Even if the checks fail, the execution of the script doesn’t stop. 

[Thresholds](https://k6.io/docs/using-k6/thresholds/), on the other hand, do interrupt the script’s execution. They’re criteria you can use to fail your load test script when the system under test doesn’t meet the expectations.

Here are some examples of thresholds you could use:

- The system generates at the most 1% errors.
- Response time for 99% of requests should be below 400ms.
- Response time for a specific endpoint must always be below 300ms.

Since k6 allows you to define custom metrics, you can always define thresholds on them as well. Here’s an example of what thresholds look like in an actual script:

```javascript
export let options = {
  thresholds: {
    http_req_failed: ['rate<0.01'],   // http errors should be less than 1% 
    http_req_duration: ['p(99)<400'], // 99% of requests should be below 400ms
  },
};
```

## k6: When Unit Tests Meet Load Testing

In this post, you’ve learned how to get started with load testing using k6. We’ve covered some fundamentals on load testing: you’ve learned the definition of this technique, and the differences between scriptable and non-scriptable load testing tools.

k6 belongs to the latter group. It allows developers to author test scenarios in JavaScript—a language they’re likely to already know. Because of that, k6 is a powerful tool, enabling the performing not only of HTTP benchmarking, but also the verification of realistic usage scenarios.

Perhaps the greatest differentiator of k6 is that it bridges the gap between unit testing and load testing. Developers can use the same workflow they’ve been using for years: creating tests, adding verifications with pass/fail criteria, adding those to the CI/CD pipeline, and then be notified when the tests do fail.