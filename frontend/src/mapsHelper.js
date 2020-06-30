let numRequests = 0;
let currentTimeout = 0;
let lastTime = 0;
export default {
    RequestMap(method, arg1)
    {
        numRequests++;
        if(numRequests < 9)
        {
            method(arg1);
            lastTime = Date.now();
        }
        else{
            if(Date.now() - lastTime > 1200)
            {
                method(arg1)
                lastTime = Date.now();
                currentTimeout = 0;
            }
            else{
                currentTimeout += 1200;
                setTimeout(() => { this.PerformRequest(method, arg1)}, currentTimeout);
            }
        }
    },
    PerformRequest(method, arg1)
    {
        currentTimeout -= 1200;
        method(arg1)
    }
};
  