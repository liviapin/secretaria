Iniciando a Aplica��o

Para subir todos os servi�os necess�rios (API, banco de dados, etc.), utilize o Docker Compose com o seguinte comando:
```bash
docker compose up
```
Usu�rio Administrador Padr�o

Ap�s aplicar a migration inicial, ser� criado automaticamente um usu�rio administrador padr�o, �til para testes e valida��es durante o desenvolvimento da aplica��o.

Credenciais:

Email: admin@admin.com

Senha: Senha@123

Acesse o Swagger UI:
http://localhost:{porta}/swagger

Acesse a rota de login via POST:

POST /api/auth/login

Corpo da requisi��o (JSON):

```json
{
  "email": "admin@admin.com",
  "senha": "Senha@123"
}
```

Copie o TOKEN da resposta, para utilizar no pr�ximo passo.

Clique em "Authorize".

No campo exibido, informe o token JWT da seguinte forma:

Bearer SEU_TOKEN_AQUI

Ap�s autorizar, todas as rotas protegidas estar�o dispon�veis para testes diretamente no Swagger.