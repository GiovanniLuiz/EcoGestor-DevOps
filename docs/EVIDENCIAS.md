# Roteiro de evidências reais

Não substitua execuções por imagens ilustrativas. Os prints da Fase 5 demonstram MongoDB,
não os requisitos DevOps desta fase. Nesta versão nenhum deploy foi declarado como realizado.

| Código | Capturar | Critério de conferência |
|---|---|---|
| E01 | Job Build aberto no GitHub | Commit e conclusão bem-sucedida |
| E02 | Etapa Testes automatizados | Contagem de testes e zero falhas |
| E03 | Build da imagem e teste Compose | API pronta com SQL Server |
| E04 | Job staging | Deploy e verificações aprovados |
| E05 | Navegador em 8081 | /health/ready com Staging e versão; /api/unidades com dados |
| E06 | Job production | Executado depois do sucesso de staging |
| E07 | Navegador em 8082 | /health/ready com Production e mesma versão; /api/unidades |
| E08 | Docker Desktop | Projetos, containers e volumes separados |

Salve as imagens em `docs/evidencias/`. Insira no README usando, por exemplo,
`![Build aprovado](docs/evidencias/E01-build.png)` somente depois de o arquivo existir.
Registre o link real da execução do GitHub e o SHA do commit. Não capture senhas, tokens
ou conteúdo do .env. Artefatos `testes`, `deploy-staging` e `deploy-production` também podem
ser baixados do workflow e usados para conferir as informações do relatório.

Na seção de desafios, descreva apenas problemas observados e soluções realmente aplicadas.
Antes da entrega, inserir os prints no PDF, atualizar o status de validação e o checklist,
e gerar novamente o ZIP final. O PDF de preparação não contém esses prints ainda.
