..\..\packages\OpenCover.4.5.3522\OpenCover.Console.exe -register:user -target:"C:\Program Files (x86)\NUnit 2.6.3\bin\nunit-console.exe" -targetargs:"/noshadow ..\bin\Debug\ConfigGenerator.Core.Tests.Unit" -filter:+[*]* -output:"Data\coverage.xml"

..\..\packages\ReportGenerator.2.0.4.0\ReportGenerator.exe -reports:"Data\coverage.xml" -targetdir:"Report" 