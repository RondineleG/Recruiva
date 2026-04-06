# 📜 REGRAS DE NEGÓCIO - RECRUIVA

## 1. CANDIDATOS (Candidates)

### 1.1 Cadastro
- **REGRA 001**: Todo candidato deve ter nome (3-100 caracteres), email válido e único, e data de nascimento
- **REGRA 002**: Candidato deve ser maior de 14 anos (data de nascimento)
- **REGRA 003**: Email deve ser único no sistema
- **REGRA 004**: Telefone é opcional, mas se informado deve ser válido (max 25 caracteres)
- **REGRA 005**: LinkedIn é opcional, mas se informado deve ser URL válida

### 1.2 Status da Conta
- **REGRA 006**: Novo candidato começa com status `Incomplete`
- **REGRA 007**: Status muda para `Active` quando dados obrigatórios estão completos
- **REGRA 008**: Status pode ser `Blocked` por administrador (motivo obrigatório)
- **REGRA 009**: Candidato bloqueado não pode se candidatar a vagas

### 1.3 Verificação
- **REGRA 010**: Email deve ser verificado via link de confirmação
- **REGRA 011**: Telefone pode ser verificado via SMS (futuro)
- **REGRA 012**: `IsEmailVerified` e `IsPhoneVerified` controlam acesso a features premium

### 1.4 Currículos
- **REGRA 013**: Candidato pode ter múltiplos currículos (Resumes)
- **REGRA 014**: Apenas UM currículo pode estar ativo (`IsActive = true`) por vez
- **REGRA 015**: Currículo ativo é o usado automaticamente em candidaturas
- **REGRA 016**: Candidato pode criar, editar e excluir seus currículos

---

## 2. ANUNCIANTES/EMPRESAS (Advertisers)

### 2.1 Cadastro
- **REGRA 020**: Todo anunciante deve ter nome (3-100 chars), email válido e único, telefone, TaxId (CNPJ/CPF) e tipo de pessoa (Física/Jurídica)
- **REGRA 021**: TaxId deve ser único no sistema
- **REGRA 022**: CPF deve ter formato válido (11 dígitos, verificadores válidos)
- **REGRA 023**: CNPJ deve ter formato válido (14 dígitos, verificadores válidos)
- **REGRA 024**: Website é opcional, mas se informado deve ser URL válida

### 2.2 Status da Conta
- **REGRA 025**: Novo anunciante começa com status `Incomplete`
- **REGRA 026**: Status muda para `Active` quando dados obrigatórios estão completos e email verificado
- **REGRA 027**: Status pode ser `Blocked` por administrador (motivo obrigatório)
- **REGRA 028**: Anunciante bloqueado não pode criar ou editar vagas

### 2.3 Plano
- **REGRA 030**: Todo anunciante tem um plano ativo (`ActivePlan`: Free, Premium, Enterprise)
- **REGRA 031**: Plano Free limita a 3 vagas ativas simultâneas
- **REGRA 032**: Plano Premium permite vagas ilimitadas + boost/highlight
- **REGRA 033**: Plano Enterprise permite vagas ilimitadas + features exclusivas + multi-tenant dedicado

### 2.4 Perfil Público
- **REGRA 034**: Anunciante pode ter logo (URL) e descrição da empresa
- **REGRA 035**: Logo e descrição são exibidos nas vagas publicadas

---

## 3. VAGAS (Jobs)

### 3.1 Criação
- **REGRA 040**: Apenas anunciantes com status `Active` podem criar vagas
- **REGRA 041**: Título da vaga é obrigatório (3-200 caracteres)
- **REGRA 042**: Descrição da vaga é obrigatória (10-2000 caracteres)
- **REGRA 043**: Data de expiração deve ser futura
- **REGRA 044**: Requisitos, responsabilidades e benefícios são opcionais (max 2000 chars cada)
- **REGRA 045**: Categoria é opcional (max 100 caracteres)
- **REGRA 046**: Tags são opcionais (max 500 caracteres, separadas por vírgula)

### 3.2 Localização
- **REGRA 050**: Vaga pode ser On-site, Remote ou Hybrid
- **REGRA 051**: Se On-site ou Hybrid, cidade/estado são obrigatórios
- **REGRA 052**: País padrão é `BR` (Brasil)
- **REGRA 053**: `IsRemote` indica se trabalho remoto é permitido
- **REGRA 054**: `ShowAddress` controla se endereço completo é exibido

### 3.3 Salário
- **REGRA 060**: Faixa salarial é opcional (Min/Max)
- **REGRA 061**: Moeda padrão é `BRL` (Real Brasileiro)
- **REGRA 062**: `Display = false` oculta salário na listagem
- **REGRA 063**: Se informado, Min deve ser menor que Max

### 3.4 Status
- **REGRA 070**: Nova vaga começa com status `Active` e moderação `Pending`
- **REGRA 071**: Vaga é automaticamente `Expired` quando `ExpirationDate < DataAtual`
- **REGRA 072**: Vaga pode ser `Hidden` pelo anunciante (pausa temporária)
- **REGRA 073**: Vaga pode ser `Rejected` por moderador (motivo obrigatório)
- **REGRA 074**: Vaga pode ser `Paused` pelo anunciante

### 3.5 Moderação
- **REGRA 080**: Toda nova vaga vai para moderação (`Moderation.Status = Pending`)
- **REGRA 081**: Moderador pode `Approved` ou `Rejected` vaga
- **REGRA 082**: Rejeição requer motivo (`Moderation.Reason`, max 1000 chars)
- **REGRA 083**: Moderador é identificado por `Moderation.ModeratorId`
- **REGRA 084**: Data de moderação é registrada automaticamente

### 3.6 Boost/Impulso
- **REGRA 090**: Boost só pode ser ativado por anunciantes Premium/Enterprise
- **REGRA 091**: Boost tem nível (`Featured` ou `SuperFeatured`)
- **REGRA 092**: Boost tem data de início e fim
- **REGRA 093**: Boost expira automaticamente (`Boost.IsActive = false` após `EndDate`)
- **REGRA 094**: Vaga com boost aparece no topo da listagem

### 3.7 Highlight/Destaque
- **REGRA 100**: Highlight só pode ser ativado por anunciantes Premium/Enterprise
- **REGRA 101**: Highlight tem data de início e fim
- **REGRA 102**: Highlight expira automaticamente
- **REGRA 103**: Vaga com highlight tem visual especial na listagem (borda colorida, badge)

### 3.8 Contadores
- **REGRA 110**: `Views` incrementa ao visualizar detalhes da vaga
- **REGRA 111**: `Applications` incrementa ao receber candidatura
- **REGRA 112**: `TotalJobs` registra total de vagas criadas pelo anunciante
- **REGRA 113**: `CompletedJobs` registra vagas preenchidas/fechadas

### 3.9 Limites
- **REGRA 120**: Plano Free: máximo 3 vagas ativas
- **REGRA 121**: Plano Premium: vagas ilimitadas
- **REGRA 122**: Plano Enterprise: vagas ilimitadas
- **REGRA 123**: Vagas expiradas/hidden/paused não contam no limite

---

## 4. CANDIDATURAS (Applications)

### 4.1 Criação
- **REGRA 130**: Apenas candidatos com status `Active` podem se candidatar
- **REGRA 131**: Vaga deve estar com status `Active` e moderação `Approved`
- **REGRA 132**: Candidatura é única por par (CandidateId, JobId) - impedida duplicata
- **REGRA 133**: `AppliedAt` é definido automaticamente como UTC now
- **REGRA 134**: Status inicial é `Sent`

### 4.2 Status
- **REGRA 140**: Status pode ser: `Sent`, `Viewed`, `Selected`, `Rejected`, `Archived`
- **REGRA 141**: Mudança de status é registrada em `ApplicationStatusHistory`
- **REGRA 142**: Histórico inclui status anterior, novo status, data e usuário que mudou
- **REGRA 143**: `ViewedAt`, `SelectedAt`, `RejectedAt` são definidos automaticamente ao mudar status

### 4.3 Notas
- **REGRA 150**: Recrutador pode adicionar notas à candidatura (max 1000 chars)
- **REGRA 151**: Notas são visíveis apenas para o anunciante

### 4.4 Notificações
- **REGRA 160**: Candidato recebe notificação quando status muda
- **REGRA 161**: Anunciante recebe notificação quando recebe nova candidatura
- **REGRA 162**: Notificação inclui tipo, título e mensagem

---

## 5. CURRÍCULOS (Resumes)

### 5.1 Criação
- **REGRA 170**: Apenas candidatos com status `Active` podem criar currículos
- **REGRA 171**: Título do currículo é obrigatório
- **REGRA 172**: Resumo (Summary) é opcional
- **REGRA 173**: Currículo pode estar ativo ou inativo (`IsActive`)
- **REGRA 174**: Apenas UM currículo ativo por candidato

### 5.2 Educação
- **REGRA 180**: Histórico de educação é opcional
- **REGRA 181**: Cada item deve ter instituição, curso, nível de educação
- **REGRA 182**: Data de início deve ser anterior à data de fim (se informada)
- **REGRA 183**: Status pode ser: `Ongoing`, `Completed`, `Dropped`, `Incomplete`

### 5.3 Experiência
- **REGRA 190**: Histórico de experiência é opcional
- **REGRA 191**: Cada item deve ter empresa, cargo, descrição
- **REGRA 192**: Data de início deve ser anterior à data de fim (se informada)
- **REGRA 193**: Experiência atual tem `EndDate` nulo

### 5.4 Idiomas
- **REGRA 200**: Lista de idiomas é opcional
- **REGRA 201**: Cada idioma deve ter nome e nível
- **REGRA 202**: Nível pode ser: `Basic`, `Intermediate`, `Advanced`, `Fluent`, `Native`

### 5.5 Skills
- **REGRA 210**: Lista de skills é opcional
- **REGRA 211**: Cada skill deve ter nome, nível e anos de experiência
- **REGRA 212**: Nível é numérico (1-10) ou texto
- **REGRA 213**: Anos de experiência deve ser >= 0

---

## 6. NOTIFICAÇÕES (Notifications)

### 6.1 Criação
- **REGRA 220**: Notificação é criada automaticamente em eventos específicos
- **REGRA 221**: Tipos: `NewJob`, `ApplicationStatus`, `JobExpired`, `System`, `Promotional`
- **REGRA 222**: Título e mensagem são obrigatórios
- **REGRA 223**: `RecipientId` identifica o destinatário

### 6.2 Leitura
- **REGRA 230**: Notificação começa como não lida (`IsRead = false`)
- **REGRA 231**: `ReadAt` é definido ao marcar como lida
- **REGRA 232**: Usuário pode listar apenas não lidas

### 6.3 Eventos Automáticos
- **REGRA 240**: Criar notificação quando candidatura muda de status
- **REGRA 241**: Criar notificação quando nova vaga é criada (para moderadores)
- **REGRA 242**: Criar notificação quando vaga expira (para anunciante)
- **REGRA 243**: Criar notificação quando vaga é aprovada/rejeitada (para anunciante)

---

## 7. MULTI-TENANT (TenantConfig)

### 7.1 Isolamento
- **REGRA 250**: Toda entidade tem `TenantId` obrigatório
- **REGRA 251**: Usuários só veem dados do seu tenant
- **REGRA 252**: Admin pode ver todos os tenants

### 7.2 Configurações
- **REGRA 260**: Tenant pode ter DisplayName, LogoUrl, PrimaryThemeColor customizados
- **REGRA 261**: BaseUrl permite domínio customizado (futuro)
- **REGRA 262**: Settings JSON permite configurações flexíveis
- **REGRA 263**: Tenant pode estar ativo ou inativo (`IsActive`)

---

## 8. AUDITORIA E SEGURANÇA

### 8.1 Audit Trail
- **REGRA 270**: Toda entidade registra `CreatedAt`, `CreatedBy`
- **REGRA 271**: Toda atualização registra `UpdatedAt`, `UpdatedBy`
- **REGRA 272**: Soft delete registra `DeletedAt`, `DeletedBy`, `IsDeleted = true`
- **REGRA 273**: Dados nunca são excluídos fisicamente (apenas soft delete)

### 8.2 Segurança
- **REGRA 280**: Senhas devem ter mínimo 6 caracteres, 1 maiúscula, 1 minúscula, 1 número
- **REGRA 281**: 2FA está disponível para usuários
- **REGRA 282**: JWT tokens expiram em tempo configurável
- **REGRA 283**: Refresh tokens permitem renovação automática

---

## 9. REGRAS DE MONETIZAÇÃO

### 9.1 Plano Free
- **REGRA 300**: Máximo 3 vagas ativas simultâneas
- **REGRA 301**: Sem boost ou highlight
- **REGRA 302**: 1 currículo ativo
- **REGRA 303**: Busca básica
- **REGRA 304**: Notificações por email

### 9.2 Plano Premium (R$ 29,90/mês)
- **REGRA 310**: Vagas ilimitadas
- **REGRA 311**: Boost de vagas disponível
- **REGRA 312**: Highlight disponível
- **REGRA 313**: Analytics básico
- **REGRA 314**: Múltiplos currículos
- **REGRA 315**: Filtros avançados

### 9.3 Plano Enterprise (R$ 99,90/mês)
- **REGRA 320**: Tudo do Premium
- **REGRA 321**: Multi-tenant dedicado
- **REGRA 322**: Customização de marca completa
- **REGRA 323**: API REST
- **REGRA 324**: Relatórios customizados
- **REGRA 325**: Gerenciamento de equipe
- **REGRA 326**: SLA garantido

### 9.4 Serviços Avulsos
- **REGRA 330**: Boost avulso: R$ 9,90 por 7 dias
- **REGRA 331**: Highlight avulso: R$ 4,90 por 7 dias
- **REGRA 332**: Vaga urgente: R$ 19,90 por 3 dias (topo absoluto)
- **REGRA 333**: Acesso ao banco de currículos: R$ 49,90/mês

---

## 10. REGRAS DE VALIDAÇÃO CRÍTICA

### 10.1 Impedimentos
- **REGRA 400**: Candidato bloqueado NÃO pode se candidatar
- **REGRA 401**: Anunciante bloqueado NÃO pode criar/editar vagas
- **REGRA 402**: Vaga expirada NÃO recebe candidaturas
- **REGRA 403**: Vaga rejeitada NÃO aparece na listagem pública
- **REGRA 404**: Candidatura duplicada é IMPEDIDA
- **REGRA 405**: Vaga além do limite do plano é IMPEDIDA

### 10.2 Alertas
- **REGRA 410**: Candidato com email não verificado recebe alerta
- **REGRA 411**: Anunciante com vagas prestes a expirar recebe alerta
- **REGRA 412**: Candidato sem currículo ativo recebe sugestão para criar

---

## 11. REGRAS DE EXPIRAÇÃO AUTOMÁTICA

### 11.1 Vagas
- **REGRA 420**: Vaga expira quando `ExpirationDate < DataAtual UTC`
- **REGRA 421**: Status muda automaticamente para `Expired`
- **REGRA 422**: Notificação é enviada ao anunciante
- **REGRA 423**: Vaga expirada sai da listagem pública

### 11.2 Boost/Highlight
- **REGRA 430**: Boost expira quando `Boost.EndDate < DataAtual UTC`
- **REGRA 431**: `Boost.IsActive` muda para `false`
- **REGRA 432**: Highlight expira quando `Highlight.EndDate < DataAtual UTC`
- **REGRA 433**: `Highlight.IsActive` muda para `false`

---

## 12. REGRAS DE NEGÓCIO ESPECÍFICAS

### 12.1 Busca de Vagas
- **REGRA 440**: Busca textual pesquisa em título, descrição e categoria
- **REGRA 441**: Filtros podem ser combinados (localização, salário, tipo)
- **REGRA 442**: Resultados ordenados por relevância, data ou salário
- **REGRA 443**: Paginação padrão: 20 vagas por página
- **REGRA 444**: Apenas vagas `Active` e `Approved` aparecem na busca

### 12.2 Dashboard
- **REGRA 450**: Candidato vê vagas recomendadas (baseado em skills/localização)
- **REGRA 451**: Candidato vê status das suas candidaturas
- **REGRA 452**: Anunciante vê métricas das suas vagas (views, aplicações)
- **REGRA 453**: Admin vê métricas globais do sistema

### 12.3 Perfil Público
- **REGRA 460**: Candidato pode ter perfil público visível (opcional)
- **REGRA 461**: Anunciante tem perfil público com logo e descrição
- **REGRA 462**: Vagas exibem perfil do anunciante

---

## 📊 RESUMO DE REGRAS POR ENTIDADE

| Entidade | Nº Regras | Críticas | Importantes |
|----------|-----------|----------|-------------|
| **Candidate** | 16 | 4 | 12 |
| **Advertiser** | 16 | 4 | 12 |
| **Job** | 44 | 12 | 32 |
| **Application** | 14 | 6 | 8 |
| **Resume** | 14 | 4 | 10 |
| **Notification** | 10 | 3 | 7 |
| **TenantConfig** | 7 | 2 | 5 |
| **Monetização** | 18 | 4 | 14 |
| **Validação** | 13 | 6 | 7 |
| **Expiração** | 8 | 4 | 4 |
| **TOTAL** | **160** | **49** | **111** |

---

*Documento gerado em: 06/04/2026*
*Versão: 1.0*
