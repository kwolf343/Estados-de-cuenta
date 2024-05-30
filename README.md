#Prueba tecnica Estados de cuenta banco Atlantida
## Autor: Kevin Antonio Magaña Monroy

## Descripcion:
Esta prueba consta de dos proyectos: Api rest para el backend y Front end con mvc
Para el correcto funcionamiento del proyecto deben estar funcionando tanto la api como el backend (aun que el front está preparado para funconar en caso que la api esté apagada)

## Instrucciones:
1- Ejecutar el script sql "estadosCuenta.sql" este creará una base de datos llamada "estadosCuenta" con dos tablas, 6 procedimientos almacenados y 2 inserts de ejemplo para estados de cuenta
2- En el proyecto ApiRestEDC en el archivo appSettings.json se verá la cadena de contexión (modificar en caso de ser necesario)
3- Una vez teniendo la base de datos creada y el proyecto ApiRestEDC corriendo, se podrá hacer uso del api Rest el cual cuenta con 5 end points los cuales son: GET /EstadoCuentas, POST /EstadoCuentas, GET /EstadoCuentas/{id}, GET /DetalleEstadoCuentas, POST /DetalleEstadoCuentas/{id}
4- Iniciar el proyecto FrontEndEDC el cual deberá mostrar los dos estados de cuenta de ejemplo insertados en la base de datos

## Detalles
- El front end no crea estados de cuenta (por lo que de necesitar uno nuevo se deberá crear desde el api rest)
- El Api cuenta con documentación swagger
