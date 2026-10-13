# Validação da entrega final

Data das evidências: 13/10/2026. Autor: Giovanni Luiz do Rego - RM 563071.
Versão executada: `42fd1f59bb92aef482382bbffd3327c93bcff436`.

## Resultados observados

- Build local e testes locais aprovados na preparação.
- GitHub Actions: workflow com status Success; build-test, staging e production aprovados.
- Testes no GitHub: 12 aprovados, 0 falhas, 0 ignorados; duração informada de 6 segundos.
- Job build-test: 2min10s; staging/deploy: 2min45s; production/deploy: 2min54s.
- Tempo total do workflow: 36min12s, incluindo espera pelo runner; não representa apenas processamento.
- Quatro artefatos indicados no resumo da execução.
- Staging: localhost:8081/health/ready retorna ready, ambiente Staging e versão do commit.
- Produção simulada: localhost:8082/health/ready retorna ready, ambiente Production e a mesma versão.
- /api/unidades retorna duas unidades nos dois ambientes.
- Docker Desktop exibe API e SQL Server dos dois ambientes em execução, além do ambiente local.
- Volumes distintos: ecogestor-local_sql-data, ecogestor-staging_sql-data e ecogestor-production_sql-data.
- Relatório protegido retornou HTTP 200 na verificação local via Swagger.

As evidências foram fornecidas pelo autor e estão indexadas em EVIDENCIAS.md. Esta finalização
atualizou documentação e empacotamento; não executou um novo deploy nem repetiu os testes de código.
O JSON validacao-local.json preserva o registro histórico anterior ao deploy; os indicadores false
nesse arquivo descrevem apenas aquele momento. Os resultados finais estão neste documento.

## Escopo e limites

Staging e Production são ambientes acadêmicos locais em Docker Desktop, com SQL Server Developer.
Não há hospedagem pública. O runner Windows ecogestor-windows foi iniciado interativamente,
com as labels self-hosted, Windows, X64 e ecogestor. Para novos deploys, ele precisa estar ativo.
As capturas demonstram volumes separados, mas não constituem um teste de restauração de backup
ou de recuperação após remoção dos containers. Não há rollback automático.
O resumo do GitHub registra três warnings e um notice; o resultado final foi Success.

## Conferência do pacote

O ZIP inclui código-fonte, testes, Dockerfile, Compose, workflows, scripts, .env.example,
README, documentação final e capturas. Exclui .env real, .git, bin, obj, resultados temporários
e arquivos de imagem Docker. O histórico de preparação não deve ser confundido com a validação final.
