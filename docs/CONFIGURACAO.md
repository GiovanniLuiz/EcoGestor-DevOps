# Configuração do GitHub e dos dois ambientes

Este guia prepara a execução; não é evidência de deploy realizado.

## 1 Preparar o computador

Instale Docker Desktop para Windows x64, com WSL 2 e Linux containers.
Execute primeiro os comandos locais do README. Feche ou pare `ecogestor-local`
antes de manter staging e produção, para economizar memória.
O serviço SQL Server precisa de tempo para iniciar na primeira execução.

## 2 Preparar o repositório

Crie um repositório GitHub e envie o conteúdo da pasta EcoGestorDevOps,
com o README e Dockerfile na raiz. Mantenha a pasta oculta `.github`.
Não envie `.env`, bin, obj ou artefatos de imagem. Use a branch `main`.
O pipeline de build pode rodar antes de configurar o runner; os jobs de deploy
ficarão esperando o runner/segredos. Isso não significa que a entrega está concluída.

## 3 Configurar os environments

Em Settings > Environments crie `staging` e `production`.
A disponibilidade dessas opções depende do plano e da visibilidade do repositório.
Se a opção não aparecer, verifique o plano antes de alterar a visibilidade.
Em cada environment, cadastre secrets diferentes:

- DB_PASSWORD: 16 a 128 caracteres; maiúsculas, minúsculas, números e símbolo; use apenas letras, números e ! _ @ # -.
- JWT_KEY: pelo menos 32 bytes; recomendado 64 caracteres hexadecimais aleatórios.
- ADMIN_PASSWORD: pelo menos 12 caracteres; use uma senha aleatória.

Para gerar valores localmente em PowerShell (não incluir no print):

```powershell
'Eg9!' + [Guid]::NewGuid().ToString('N') + [Guid]::NewGuid().ToString('N')
[Guid]::NewGuid().ToString('N') + [Guid]::NewGuid().ToString('N')
'Eg9!' + [Guid]::NewGuid().ToString('N') + [Guid]::NewGuid().ToString('N')
```

As três linhas correspondem, na ordem, aos três secrets. Gere novamente para o outro
environment. Guarde as senhas em local seguro. Não altere DB_PASSWORD ou ADMIN_PASSWORD
após o primeiro deploy sem um procedimento de troca também dentro do banco.
O fluxo padrão promove automaticamente após staging. Uma regra de aprovação manual
adicionada no GitHub pausará a promoção até a aprovação.

## 4 Configurar o runner local

Em Settings > Actions > Runners > New self-hosted runner escolha Windows x64.
Siga os comandos exibidos pelo GitHub em uma pasta dedicada, fora do projeto.
Adicione a etiqueta `ecogestor`, além de `self-hosted`, `Windows` e `X64`.
Não compartilhe o token de registro do runner.

Execute o runner de forma interativa com `run.cmd`, usando o mesmo usuário que executa
Docker Desktop. Mantenha sua janela aberta. Rodar como serviço sob outra conta pode impedir
o acesso ao Docker Desktop. Confira `docker version` e `docker compose version` nessa conta.
As pastas do runner e seus arquivos locais devem ser acessíveis apenas a usuários confiáveis.
O deploy guarda configuração com segredos em `%LOCALAPPDATA%/EcoGestorDevOps`;
essa pasta não vai para o repositório nem para o ZIP.

O runner de deploy só recebe jobs de push/manual em `main`; PRs rodam exclusivamente em
máquinas hospedadas do GitHub. Não autorize colaboradores desconhecidos a alterar `main`
ou workflows: código executado no runner pode acessar o computador e os segredos.

## 5 Executar e conferir

Faça push em `main` ou abra Actions > EcoGestor CI CD > Run workflow, na branch main.
Acompanhe build-test, staging e production. A imagem aprovada é transferida como artefato,
sem exigir conta Docker Hub ou registry externo.

No computador com Docker, abra:

- http://localhost:8081/health/ready e http://localhost:8081/swagger
- http://localhost:8082/health/ready e http://localhost:8082/swagger

As respostas devem mostrar Staging e Production e o mesmo SHA em `version`.
Confira também `/api/unidades` nos dois endereços. Os URLs localhost não abrem
no computador do professor; inclua prints legíveis e os links de execução do pipeline.
Se houver exigência adicional de acesso público na avaliação, adapte a hospedagem antes da entrega.

## 6 Operação e problemas comuns

- Runner offline: abra Docker Desktop e `run.cmd`; confira as etiquetas.
- Job sem secrets: confira os nomes e o environment de cada job.
- SQL unhealthy: confira memória, logs do banco e política da senha.
- Login falha após trocar secret: o usuário existente mantém a senha criada no primeiro deploy.
- Porta ocupada: libere 8081/8082 ou altere o parâmetro no workflow e a documentação.
- Pipeline falhou: corrija a causa, faça novo commit e execute novamente; não marque o checklist como concluído.

Depois de concluir as evidências, os containers podem ser parados sem remover volumes:

```powershell
docker compose --env-file "$env:LOCALAPPDATA/EcoGestorDevOps/staging.env" -p ecogestor-staging stop
docker compose --env-file "$env:LOCALAPPDATA/EcoGestorDevOps/production.env" -p ecogestor-production stop
```

Não existe rollback automático nem backup externo neste laboratório. Para uso real,
planeje backups, acesso SQL com privilégio mínimo, TLS, monitoramento e estratégia de rollback.
