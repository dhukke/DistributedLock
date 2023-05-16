# Distributed lock

Using redis

## Run local

Run the command `docker-compose up` on the root folder to spin up redis (localhost:6379)  and redis commander (http://localhost:8081/)

Add to the secrets.json:

```json
{
  "ConnectionStrings:Redis": "localhost:6379"
}
```

Run the app.