Build using [IdentityManagerUI](https://github.com/mguinness/IdentityManagerUI) check it out.
This project is a fork with upgrades to .NET6 , Bootstrap 5 and Datatables 5

## Getting started WITHOUT .NET6 local development installed
* Clone repository
* Instal docker https://docs.docker.com/get-docker/
* go to the directory of the project
* Run the following command:
```bash
docker-compose up
```
* login with Admin account
* Navigate to Users or Roles to see it in action

## Getting started WITH .NET6 local development installed

* Clone repository
* Open in visual studio. Ensure you have .net 6, asp net core 6 and identity components added (might all be from the C#/net core 6 web development workload).
  * the project is also configured to use MSSQL, via localdb for testing. This should be installed with visual studio. Change if required
* allow it to restore dependencies
* Tools > Package Manager > Console
* Run `Update-Database` to apply the migrations
* `F5` to run the project
* login with Admin account
* Navigate to Users or Roles to see it in action

## Notes
* This project automatically seeds admin user data, to disable this behaivour go to appsettings.json, and change to false
```json
 "Identity": {
    "SeedInitialUserData":  true
  }
```

