# Guía de Desarrollo Local — BBVA Legalización

## Requisitos previos

| Herramienta | Versión | Descarga |
|---|---|---|
| Docker Desktop | Última estable | https://www.docker.com/products/docker-desktop |
| .NET SDK | **8.0.418** | https://dotnet.microsoft.com/en-us/download/dotnet/8.0 |
| Node.js | 20+ | https://nodejs.org/ |
| Git | Última estable | https://git-scm.com/ |

> Si ya tenés otra versión del SDK de .NET (como la 10), instalá la 8 en paralelo. El archivo `backend/global.json` selecciona automáticamente la 8 cuando estés en esa carpeta.

Verificar instalación:

```bash
dotnet --list-sdks        # Debe mostrar 8.0.418 (entre otras)
node --version            # v20.x o superior
docker --version          # Docker Desktop corriendo
```

---

## Estructura del proyecto

```
will-fuentes/
├── backend/                 # API .NET 8 + docker-compose (Postgres, Mongo)
│   ├── Multibanca.Backend.Api/   # Proyecto principal de la API
│   ├── docker-compose.yml        # Servicios: api, postgres, mongo
│   ├── .env                      # Variables de entorno (local)
│   └── ...                       # Capas de dominio, aplicación, datos
└── frontend/                # React 19 + Vite + TypeScript
    ├── package.json
    ├── .env                 # VITE_API_BASE_URL apunta al backend
    └── ...
```

---

## Paso 1 — Levantar las bases de datos

Desde la carpeta `backend`, levantá PostgreSQL y MongoDB con Docker:

```bash
cd backend
docker compose up -d postgres mongo
```

Esto inicia:
- **PostgreSQL 17** en `localhost:5432` (usuario: `postgres` / contraseña: `postgres123`)
- **MongoDB 7** en `localhost:27017` (usuario: `multibanca` / contraseña: `multibanca123`)

Verificar que estén corriendo:

```bash
docker compose ps
```

> Los datos se persisten en `C:\Docker\PostgresData` y `C:\Docker\MongoData`. Si no existen, creálas:
> ```bash
> mkdir C:\Docker\PostgresData
> mkdir C:\Docker\MongoData
> ```

---

## Paso 2 — Configurar la base de datos

La primera vez necesitás crear el usuario y las bases de datos:

```bash
# Crear el usuario multibanca
docker exec -it postgres psql -U postgres -c "CREATE USER multibanca WITH PASSWORD 'bbva123';"

# Crear las bases de datos con owner multibanca
docker exec -it postgres psql -U postgres -c "CREATE DATABASE BBVA_LEGALIZACION OWNER multibanca;"
docker exec -it postgres psql -U postgres -c "CREATE DATABASE DBWFBBVA OWNER multibanca;"

# Otorgar permisos completos
docker exec -it postgres psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE BBVA_LEGALIZACION TO multibanca;"
docker exec -it postgres psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE DBWFBBVA TO multibanca;"
```

> El `backend/.env` ya viene configurado con el usuario `multibanca`, no necesitás modificar nada.

### Restaurar los backups

El proyecto incluye backups en `base_datos/base_datos/`. Para cargarlos:

```bash
# Copiar los backups al contenedor
docker cp "base_datos\base_datos\bbva_legalizacion.backup" postgres:/tmp/bbva_legalizacion.backup
docker cp "base_datos\base_datos\Backup Multibanca BBVA.backup" postgres:/tmp/multibanca_bbva.backup

# Restaurar BBVA_LEGALIZACION
docker exec -it postgres pg_restore -U multibanca -d bbva_legalizacion --no-owner --no-privileges /tmp/bbva_legalizacion.backup

# Restaurar DBWFBBVA
docker exec -it postgres pg_restore -U multibanca -d dbwfbbva --no-owner --no-privileges /tmp/multibanca_bbva.backup
```

> Si te da error de tablas existentes, agregá `--clean` para que las elimine antes de restaurar:
> ```bash
> docker exec -it postgres pg_restore -U multibanca -d bbva_legalizacion --no-owner --no-privileges --clean /tmp/bbva_legalizacion.backup
> ```

### Configurar contraseña de usuarios

Las contraseñas en la base están hasheadas (SHA512 + salt). Para poder loguearte, actualizá la contraseña de todos los usuarios a `password`:

```bash
docker exec -it postgres psql -U multibanca -d bbva_legalizacion -c "UPDATE users SET password = 'R+qQ4GIEHD8aOOyo32B11wjGpqNMs/Lm8qQ0HIchwHxLYvZfHefks/s3vFnmq9qCksTxfmO4tTzkfhelVJlOtw==sMVoX3NdnjWLmZeE8cgt7yHrpvKTUi0PAYOGz1u6DFaRl54k';"
```

Para ver los usuarios disponibles:

```bash
docker exec -it postgres psql -U multibanca -d bbva_legalizacion -c "SELECT user_id, user_name, email FROM users;"
```

Después te logueás con cualquier `user_name` de la lista y contraseña **`password`**.

> **Contraseña maestra:** El código tiene un bypass de desarrollo. Podés loguearte con cualquier usuario usando la contraseña `@Ciber2026` sin necesidad de actualizar el hash en la base.

---

## Paso 3 — Levantar el backend (con hot reload)

```bash
cd backend/Multibanca.Backend.Api
dotnet watch run
```

La API arranca en:
- **HTTP:** http://localhost:5280 (recomendado para desarrollo)
- **HTTPS:** https://localhost:44302
- **Swagger:** http://localhost:5280/swagger

Cada vez que guardes un archivo `.cs`, se recompila automáticamente.

---

## Paso 4 — Levantar el frontend (con hot reload)

```bash
cd frontend
npm install          # Solo la primera vez o cuando cambie package.json
npm run dev
```

> **Nota:** Si `npm install` falla con error E401 de autenticación, es porque tu `.npmrc` global apunta a un registry privado (CodeArtifact). Creá un archivo `frontend/.npmrc` con el siguiente contenido:
> ```
> registry=https://registry.npmjs.org/
> ```
> Esto fuerza el uso del registry público de npm solo para este proyecto.

Vite arranca en **http://localhost:5173** con hot reload.

El frontend apunta al backend en `http://localhost:5280` (configurado en `frontend/.env`).

---

## Resumen de puertos

| Servicio | URL | Hot reload |
|---|---|---|
| Frontend (Vite) | http://localhost:5173 | ✓ |
| Backend API | http://localhost:5280 | ✓ |
| Swagger | http://localhost:5280/swagger | — |
| PostgreSQL | localhost:5432 | — |
| MongoDB | localhost:27017 | — |

---

## Comandos útiles

```bash
# Detener las bases de datos
cd backend
docker compose down

# Detener y eliminar datos (reset completo)
docker compose down -v

# Ver logs del postgres
docker compose logs -f postgres

# Reconstruir todo con Docker (sin hot reload, modo producción)
docker compose up --build -d

# Lint del frontend
cd frontend
npm run lint

# Build de producción del frontend
npm run build
```

---

## Solución de problemas

### El backend no arranca — "global.json requires SDK 8.0.418"

Instalá el SDK 8.0.418 desde https://dotnet.microsoft.com/en-us/download/dotnet/8.0. Convive con otras versiones sin problema.

### Error de conexión a PostgreSQL

Verificá que el container esté corriendo:
```bash
docker compose ps
```
Y que las credenciales en `backend/.env` coincidan con las del compose.

### El frontend no conecta con el backend

Revisá que `frontend/.env` tenga:
```
VITE_API_BASE_URL = http://localhost:5280
```
Y que el backend esté corriendo.

### Error de certificado HTTPS en desarrollo

El backend usa un certificado de desarrollo. Si el navegador bloquea la conexión, aceptá el certificado o usá el perfil HTTP (`http://localhost:5280`) temporalmente.

---

## Variables de entorno

Las variables sensibles están en `backend/.env` (excluido de Git). Valores importantes para desarrollo local:

| Variable | Valor por defecto |
|---|---|
| ConnectionStrings__Default | Host=localhost;Port=5432;Database=BBVA_LEGALIZACION;... |
| ConnectionStrings__dbWorkFlow | Host=localhost;Port=5432;Database=DBWFBBVA;... |
| MongoDb__ConnectionString | mongodb://multibanca:bbva123@localhost:27017 |
| Jwt__Issuer | MULTIBANCA_USERS |
| VITE_AUTH_USE_SAML | false (login local) |
