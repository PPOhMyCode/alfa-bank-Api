docker -H=your-remote-server.org:2375 %*
docker ^
  --tlsverify ^
  -H=your-remote-server.org:2376 ^
  --tlscacert=C:\users\me\docker-tls\ca.pem ^
  --tlscert=C:\users\me\docker-tls\cert.pem ^
  --tlskey=C:\users\me\docker-tls\key.pem %*