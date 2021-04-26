# ProvueCLI
![Build badge](https://ci.appveyor.com/api/projects/status/yfi251r9ahygs420?svg=true) [![codecov](https://codecov.io/gh/P-RCollaboration/ProvueCLI/branch/main/graph/badge.svg?token=GUQFKDOKRV)](https://codecov.io/gh/P-RCollaboration/ProvueCLI) ![license](https://img.shields.io/badge/license-MIT-green)  
The command-line application that helps in development with Provue

### Features

* Build a component file tree for development - file names for components are currently supported in the development console in a web browser.
* Build for release - minification and uglifying for html, css and js sections
* Run development static web server with file watch feature

```
Example usage: ProvueCLI <arument1> <arument2> <arument3>

  Options  
  
  sourcefolder:<full or relative path> - specify folder that a contains source code
  buildfolder:<full or relative path> - specify folder where will be processed source code for development
  releasefolder:<full or relative path> - specify folder where will be processed source code for release
  serverfolder:<full or relative path> - specify folder that will be mapped in web server for static
  port:<full or relative path> - port for web server for static (default 8080)
  host:<full or relative path> - host for web server for static (default localhost)
  
  Commands  
  
  buildrelease - indicate that need build release version (you need specify option releasefolder)
  run - indicate that need run web server for static
```
