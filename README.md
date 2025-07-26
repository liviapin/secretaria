Iniciando a Aplicação

Para subir todos os serviços necessários (API, banco de dados, etc.), utilize o Docker Compose com o seguinte comando:
```bash
docker compose up
```
Usuário Administrador Padrão

Após aplicar a migration inicial, será criado automaticamente um usuário administrador padrão, útil para testes e validações durante o desenvolvimento da aplicação.

Credenciais:

Email: admin@admin.com

Senha: Senha@123

Acesse o Swagger UI:
http://localhost:{porta}/swagger

Acesse a rota de login via POST:

POST /api/auth/login

Corpo da requisição (JSON):

```json
{
  "email": "admin@admin.com",
  "senha": "Senha@123"
}
```

Copie o TOKEN da resposta, para utilizar no próximo passo.

Clique em "Authorize".

No campo exibido, informe o token JWT da seguinte forma:

Bearer SEU_TOKEN_AQUI

Após autorizar, todas as rotas protegidas estarão disponíveis para testes diretamente no Swagger.