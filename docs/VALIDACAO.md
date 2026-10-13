# Registro de validação

Data: 13/10/2026. Validação executada nesta máquina Windows com SDK .NET 8.0.425.

## Resultados confirmados

- Build da solução em Release: aprovado, zero erros e zero avisos.
- Testes: 12 aprovados, zero falhas e zero ignorados.
- Docker Compose: configuração aceita por `docker compose config --quiet` (Compose 5.6.0).
- GitHub Actions: workflows aceitos pelo actionlint 1.7.12; verificações externas shellcheck/pyflakes desabilitadas.
- PowerShell: os três scripts passaram pela análise sintática do PowerShell.

| Teste | Resultado |
|---|---|
| EcoGestorApi.Tests.DevOpsTests.Readiness_ReportsEnvironment | Passed |
| EcoGestorApi.Tests.DevOpsTests.SqlServerMigration_IsDiscoverableAndGeneratesTables | Passed |
| EcoGestorApi.Tests.DestinacoesResiduoControllerTests.Get_ReturnsHttpStatusCode200 | Passed |
| EcoGestorApi.Tests.DevOpsTests.ProtectedEndpoint_WithoutToken_Returns401 | Passed |
| EcoGestorApi.Tests.AuthControllerTests.Login_ReturnsHttpStatusCode200 | Passed |
| EcoGestorApi.Tests.AlertasColetaControllerTests.Get_ReturnsHttpStatusCode200 | Passed |
| EcoGestorApi.Tests.PontosColetaControllerTests.Get_ReturnsHttpStatusCode200 | Passed |
| EcoGestorApi.Tests.DevOpsTests.WrongPassword_Returns401 | Passed |
| EcoGestorApi.Tests.RelatoriosControllerTests.Get_ReturnsHttpStatusCode200 | Passed |
| EcoGestorApi.Tests.DevOpsTests.Unit_CanBeCreatedUpdatedAndDeleted | Passed |
| EcoGestorApi.Tests.UnidadesControllerTests.Get_ReturnsHttpStatusCode200 | Passed |
| EcoGestorApi.Tests.ColetasResiduoControllerTests.Get_ReturnsHttpStatusCode200 | Passed |

## Limitações e pendências

Os testes HTTP usam EF InMemory, e a migration é validada por geração de SQL. Não foi estabelecida conexão com SQL Server real nesta validação.

Docker Desktop/Engine não está instalado neste computador. Não foram executados o build da imagem, containers ou healthchecks Docker. A validação de Compose confirma configuração, não funcionamento.

Nenhum workflow foi enviado ou executado no GitHub. Nenhum runner foi registrado. Deploy em staging e produção e respectivas evidências continuam pendentes.

O pipeline inclui um teste de integração com SQL Server em container no GitHub e verificações após cada deploy. Esses passos só serão confirmados depois da execução real.

## Problemas encontrados e ajustes aplicados

- Testes originais usavam atributos xUnit sem importação global; adicionada a importação.
- Configuração de banco dos testes podia combinar provedores SQL Server e InMemory; isolada por ambiente e fixture.
- Snapshot de migrations original não descrevia o modelo completo; gerada migration inicial com metadados e snapshot completos para novos bancos.
- Credenciais fixas e senha em texto simples substituídas por configuração de ambiente e hash de senha.
- Configuração JWT carregada no momento de criação das opções de autenticação, permitindo configurar o host de teste corretamente.
- Corrigido o escape do comando de healthcheck e validado o YAML do Compose com a ferramenta oficial.

O registro acima é evidência textual de validação local. Não equivale a prints de deploy remoto.
