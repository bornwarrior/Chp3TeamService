rm -rf _builds _steps _projects _cache _temp
wercker build --docker-host "unix:///var/run/docker.sock" --git-domain github.com    --git-owner bornwarrior   --git-repository Chp3TeamService
rm -rf _builds _steps _projects _cache _temp

