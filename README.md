#Digevo-Viral-Core
Set de servicios para controlar el flujo de páginas de captura, datos y acciones de share.
## Requerimientos ##
 - Visual Studio 2013 o superior
 - ASP.NET 4.5
 - SQL Server 2008 Express o superior

Conceptos
=======
Campaign: Una campaña es una configuración con fecha de inicio y/o término en la cual se pueden efectuar acciones con el motor viral.

> Las acciones se configuran mediante **Triggers** que pueden ejecutarse al momento de realizar un acto de share ```OnShareIntentTriggers```, al momento de consumir un share ```OnShareCallbackTriggers``` y al momento de una conversión al consumir un share ```OnShareConversionTriggers```.

Un ```Trigger``` consiste en una llamada a una URL o servicio REST para gatillar una acción sobre una usuario. Puede configurarse para que se ejecute una vez por usuario, o varias veces. 

> Los parámetros del servicio llamado por el ```Trigger``` son manejados por el controlador respectivo, y se realizará una interpolación de los parámetros con el objeto de datos que se haya enviado, especificados como *{Campo}* : http://localhost:4343/api/Concurso?usuario={Username}&respuesta={Opciones.Pregunta4}

     


Componentes
-------
##Controllers##
- ```ShareLinkController``` sirve como enganche para gatillar una acción de share desde alguna instancia, como un portal de **DotNetNuke** u otro objeto.
- ```ViewIntentController``` reporta el acto de view o conversion a un share en particular. 
- ```LandingController``` gestiona las actividades de una landing de captura.
 
## Logging ##
Para realizar logging se usan los **Extension Methods** para ```log4net``` incluídos en la solución, como ```DebugCall```, ```InfoCall```, ```ErrorCall```.
> Los logs se guardan por defecto en C:/viralcore-logs/

Se pueden adjuntar objetos anónimos de tipo ```Func<object>``` los cuales serán interpolados al momento de escribir el log, como por ejemplo: 
```csharp
try
{
  //Do something
}catch(Exception ex)
{
  LogExtensions.Log.ErrorCall(ex, () => new { id, url, user, medium });
}
```

## Servicios ##

 - ```IUrlShortener``` permite acortar y consumir urls cortas, y hay implementaciones con distintos servicios.
 - ```SecureServices``` set de wrappers y manejos de archivos de configuración para hacer uso del servicio.

##Configuración##
Se hace uso de la arquitectura de **System.Configuration** en base a los archivos **.config**
 La sección **ConnectionStrings** debe estar definida en el archivo ```ConnectionStrings.config```
Exste una sección de configuración especial para **SecureServices** para configurar las credenciales, y puede ser especificada para distintas implementaciones. Todas deben ir marcadas como un archivo adicional el cual **no debe ser versionado**
Un ejemplo, para ```projectName```  
```xml
<?xml version="1.0"?>
<projectName.secureServicesMt>
  <credentials login="" password="" />
  <mt nc="" op="" />
</projectName.secureServicesMt>
```
y en el archivo ```Web.config``` se especificaría:
```xml
<configuration>
  <configSections>
  ...
  <sectionGroup name="secureServices">
    <section name="projectName.secureServicesMt" type="Digevo.Viral.Gateway.Models.Infrastructure.Settings.SecureServicesMtSection" allowLocation="true" allowDefinition="Everywhere" />
  </sectionGroup>
  </configSections>
  ...
  <secureServices>
    <projectName.secureServicesMt configSource="projectName.secureServicesMt.config" />
  </secureServices>
```
