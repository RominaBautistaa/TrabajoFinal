Necesito que completes la lógica del botón de las tarjetas de proyecto en mi aplicación ASP.NET Core MVC (CoWorkU). Mantén todo el diseño, estilos, colores y estructura actual.

Requiero que el botón cambie según la situación del usuario respecto al proyecto, usando las tablas existentes: Projects, ProjectMembers y JoinRequests.

Reglas que deben implementarse:

Si el usuario actual ya es miembro del proyecto (ProjectMembers), el botón debe desaparecer o mostrar un texto como “Ya eres miembro”, sin permitir volver a unirse ni enviar una solicitud.

Si el usuario ya envió una solicitud para unirse (JoinRequests) y su estado está en “Pendiente”, entonces el botón debe cambiar a “Solicitud enviada” o “Solicitud en proceso”. No debe permitir volver a enviar otra solicitud.

Si la solicitud fue rechazada y aún hay cupos disponibles, debe permitir volver a enviar una solicitud.

Si la solicitud fue aceptada, debe comportarse como el caso 1: mostrar “Ya eres miembro”.

Si el usuario todavía no es miembro, no tiene solicitudes enviadas y hay cupos disponibles (Members < MaxMembers), debe mostrarse el botón actual “Unirse al Proyecto”.

Si el proyecto está lleno (Members == MaxMembers), debe mostrarse “Cupo lleno” y deshabilitar cualquier envío de solicitud.

Usa la base de datos y las relaciones existentes, sin modificar el diseño visual ni romper ninguna funcionalidad previa. Solo agrega esta lógica de validaciones en el backend y en las vistas correspondientes.