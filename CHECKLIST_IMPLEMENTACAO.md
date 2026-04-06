# ✅ CHECKLIST DE IMPLEMENTAÇÃO - RECRUIVA

## 🔴 PRIORIDADE CRÍTICA (MVP Funcional)

### 1. Correções Imediatas
- [ ] **Unificar target framework** (Core para net9.0 ou Web para net8.0)
- [ ] **Atualizar pacotes EF Core** no Core project (8.0.17 → 9.0.7)
- [ ] **Configurar Email Sender real** (SendGrid/AWS SES) substituindo IdentityNoOpEmailSender
- [ ] **Adicionar Seed Data** (tenant padrão, usuário admin, dados de teste)

### 2. Repositórios (Implementar todos)
- [ ] `JobRepository : IBaseRepository<Job>`
- [ ] `CandidateRepository : IBaseRepository<Candidate>`
- [ ] `AdvertiserRepository : IBaseRepository<Advertiser>`
- [ ] `ApplicationRepository : IBaseRepository<Application>`
- [ ] `ResumeRepository : IBaseRepository<Resume>`
- [ ] `NotificationRepository : IBaseRepository<Notification>`
- [ ] `TenantConfigRepository : IBaseRepository<TenantConfig>`

### 3. Use Cases - Jobs (Vagas)
- [ ] `CreateJobRequest` + `CreateJobResponse` (DTOs)
- [ ] `CreateJobUseCase : IUseCase<CreateJobRequest, CreateJobResponse>`
- [ ] `UpdateJobRequest` + `UpdateJobResponse` (DTOs)
- [ ] `UpdateJobUseCase : IUseCase<UpdateJobRequest, UpdateJobResponse>`
- [ ] `GetJobByIdRequest` + `GetJobByIdResponse` (DTOs)
- [ ] `GetJobByIdUseCase : IUseCase<GetJobByIdRequest, GetJobByIdResponse>`
- [ ] `ListJobsRequest` + `ListJobsResponse` (DTOs com paginação)
- [ ] `ListJobsUseCase : IUseCase<ListJobsRequest, ListJobsResponse>`
- [ ] `SearchJobsRequest` + `SearchJobsResponse` (DTOs com filtros)
- [ ] `SearchJobsUseCase : IUseCase<SearchJobsRequest, SearchJobsResponse>`
- [ ] `DeleteJobUseCase : IUseCase<Id, RequestResult>` (soft delete)

### 4. Use Cases - Candidates (Candidatos)
- [ ] `CreateCandidateRequest` + `CreateCandidateResponse` (DTOs)
- [ ] `CreateCandidateUseCase : IUseCase<CreateCandidateRequest, CreateCandidateResponse>`
- [ ] `UpdateCandidateUseCase : IUseCase<UpdateCandidateRequest, UpdateCandidateResponse>`
- [ ] `GetCandidateByIdUseCase : IUseCase<Id, GetCandidateByIdResponse>`
- [ ] `ListCandidatesUseCase : IUseCase<ListCandidatesRequest, ListCandidatesResponse>`

### 5. Use Cases - Advertisers (Empresas)
- [ ] `CreateAdvertiserRequest` + `CreateAdvertiserResponse` (DTOs)
- [ ] `CreateAdvertiserUseCase : IUseCase<CreateAdvertiserRequest, CreateAdvertiserResponse>`
- [ ] `UpdateAdvertiserUseCase : IUseCase<UpdateAdvertiserRequest, UpdateAdvertiserResponse>`
- [ ] `GetAdvertiserByIdUseCase : IUseCase<Id, GetAdvertiserByIdResponse>`

### 6. Use Cases - Applications (Candidaturas)
- [ ] `CreateApplicationRequest` + `CreateApplicationResponse` (DTOs)
- [ ] `CreateApplicationUseCase : IUseCase<CreateApplicationRequest, CreateApplicationResponse>`
- [ ] `UpdateApplicationStatusRequest` + `UpdateApplicationStatusResponse` (DTOs)
- [ ] `UpdateApplicationStatusUseCase : IUseCase<UpdateApplicationStatusRequest, UpdateApplicationStatusResponse>`
- [ ] `ListApplicationsByJobRequest` + `ListApplicationsByJobResponse` (DTOs)
- [ ] `ListApplicationsByJobUseCase : IUseCase<ListApplicationsByJobRequest, ListApplicationsByJobResponse>`
- [ ] `ListApplicationsByCandidateRequest` + `ListApplicationsByCandidateResponse` (DTOs)
- [ ] `ListApplicationsByCandidateUseCase : IUseCase<ListApplicationsByCandidateRequest, ListApplicationsByCandidateResponse>`

### 7. Use Cases - Resumes (Currículos)
- [ ] `CreateResumeRequest` + `CreateResumeResponse` (DTOs)
- [ ] `CreateResumeUseCase : IUseCase<CreateResumeRequest, CreateResumeResponse>`
- [ ] `UpdateResumeUseCase : IUseCase<UpdateResumeRequest, UpdateResumeResponse>`
- [ ] `GetResumeByIdUseCase : IUseCase<Id, GetResumeByIdResponse>`
- [ ] `ListResumesByCandidateUseCase : IUseCase<Id, ListResumesByCandidateResponse>`

### 8. Validações de Domínio
- [ ] `JobValidator : IEntityValidator<Job>`
  - [ ] Título obrigatório (3-200 chars)
  - [ ] Descrição obrigatória (10-2000 chars)
  - [ ] Data de expiração futura
  - [ ] AdvertiserId válido
  - [ ] Categoria válida (se informada)
  
- [ ] `CandidateValidator : IEntityValidator<Candidate>`
  - [ ] Nome obrigatório (3-100 chars)
  - [ ] Email válido e único
  - [ ] Data de nascimento (maior de 14 anos)
  - [ ] Telefone válido (se informado)
  
- [ ] `AdvertiserValidator : IEntityValidator<Advertiser>`
  - [ ] Nome obrigatório (3-100 chars)
  - [ ] Email válido e único
  - [ ] Telefone obrigatório
  - [ ] TaxId (CNPJ/CPF) válido e único
  - [ ] Tipo de pessoa válido
  
- [ ] `ApplicationValidator : IEntityValidator<Application>`
  - [ ] CandidateId válido
  - [ ] JobId válido
  - [ ] Impedir candidatura duplicada (mesmo candidato + mesma vaga)
  - [ ] Vaga deve estar ativa
  
- [ ] `ResumeValidator : IEntityValidator<Resume>`
  - [ ] Título obrigatório
  - [ ] CandidateId válido
  - [ ] Pelo menos um item em Education/Experience/Skills

### 9. Páginas Blazor Essenciais
- [ ] **`/jobs`** - Listar vagas públicas (grid com filtros)
- [ ] **`/jobs/{id}`** - Detalhes da vaga + botão "Candidatar-se"
- [ ] **`/jobs/create`** - Criar vaga (requer autenticação Advertiser)
- [ ] **`/jobs/edit/{id}`** - Editar vaga (requer autenticação Advertiser)
- [ ] **`/my-jobs`** - Minhas vagas (Advertiser)
- [ ] **`/apply/{jobId}`** - Candidatar-se (requer autenticação Candidate)
- [ ] **`/my-applications`** - Minhas candidaturas (Candidate)
- [ ] **`/received-applications`** - Candidaturas recebidas (Advertiser)
- [ ] **`/dashboard`** - Dashboard (diferenciado por tipo de usuário)

### 10. Registro de Dependências (DI)
- [ ] Registrar todos os Repositórios no `Program.cs`
- [ ] Registrar todos os Use Cases no `Program.cs`
- [ ] Registrar todos os Validadores no `Program.cs`
- [ ] Configurar auto-registration via `IAssemblyScanner` (se viável)

---

## 🟡 PRIORIDADE ALTA (Funcionalidades Essenciais)

### 11. Regras de Negócio - Jobs
- [ ] **Expiração automática**: Job.Status = Expired quando ExpirationDate < Hoje
- [ ] **Moderação**: Job.Moderation.Status = Pending ao criar
- [ ] **Aprovação/Rejeição**: UseCase para moderador aprovar/rejeitar
- [ ] **Boost**: Ativar/desativar boost de vaga
- [ ] **Highlight**: Ativar/desativar destaque visual
- [ ] **Contadores**: Incrementar Views ao visualizar, Applications ao candidatar

### 12. Regras de Negócio - Applications
- [ ] **Status automático**: AppliedAt = UTC now ao criar
- [ ] **Histórico**: Criar ApplicationStatusHistory ao mudar status
- [ ] **Notificações**: Criar Notification ao mudar status
- [ ] **Impedir duplicata**: Verificar existência antes de criar

### 13. Regras de Negócio - Resumes
- [ ] **Resume ativo**: Apenas um IsActive = true por candidato
- [ ] **Educação**: Validação de datas (Start < End)
- [ ] **Experiência**: Validação de datas
- [ ] **Idiomas**: Nível válido (Basic → Native)
- [ ] **Skills**: Nível e anos de experiência válidos

### 14. Regras de Negócio - Candidates/Advertisers
- [ ] **Status automático**: Incomplete → Active quando dados completos
- [ ] **Verificação de email**: Enviar email com token de confirmação
- [ ] **Bloqueio**: Status = Blocked por admin (motivo obrigatório)

### 15. Notificações
- [ ] **CreateNotificationUseCase**: Criar notificação
- [ ] **MarkAsReadUseCase**: Marcar notificação como lida
- [ ] **ListUnreadUseCase**: Listar notificações não lidas
- [ ] **ListAllUseCase**: Listar todas notificações do usuário
- [ ] **Trigger automático**: Criar notificação quando:
  - [ ] Candidatura muda de status
  - [ ] Nova vaga criada (para moderadores)
  - [ ] Vaga expirada (para anunciante)
  - [ ] Vaga aprovada/rejeitada (para anunciante)

### 16. Páginas Blazor - Resumes
- [ ] **`/resumes`** - Listar meus currículos (Candidate)
- [ ] **`/resumes/create`** - Criar currículo
- [ ] **`/resumes/edit/{id}`** - Editar currículo
- [ ] **`/resumes/{id}`** - Visualizar currículo

### 17. Páginas Blazor - Perfil
- [ ] **`/profile/candidate`** - Editar perfil do candidato
- [ ] **`/profile/advertiser`** - Editar perfil da empresa
- [ ] **`/profile/notifications`** - Central de notificações

### 18. Busca e Filtros
- [ ] **Busca textual**: Pesquisar por título, descrição, categoria
- [ ] **Filtro por localização**: Cidade, estado, remoto
- [ ] **Filtro por salário**: Faixa min/max
- [ ] **Filtro por tipo**: On-site, Remote, Hybrid
- [ ] **Ordenação**: Mais recente, salário, relevância
- [ ] **Paginação**: 10/20/50 itens por página

### 19. Seed Data / Dados Iniciais
- [ ] **Tenant padrão**: TenantConfig com Id fixo para desenvolvimento
- [ ] **Usuário admin**: Email "admin@recruiva.com" com role "Admin"
- [ ] **Usuário teste (Candidate)**: Email "candidate@test.com"
- [ ] **Usuário teste (Advertiser)**: Email "advertiser@test.com"
- [ ] **Vagas de teste**: 5-10 vagas com dados variados
- [ ] **Candidatos de teste**: 3-5 candidatos
- [ ] **Empresas de teste**: 2-3 empresas

---

## 🟢 PRIORIDADE MÉDIA (Diferenciais)

### 20. Dashboard/Analytics
- [ ] **Dashboard Candidato**:
  - [ ] Vagas recomendadas (baseado em skills/localização)
  - [ ] Status das minhas candidaturas (gráfico)
  - [ ] Últimas vagas publicadas
  
- [ ] **Dashboard Anunciante**:
  - [ ] Total de vagas ativas
  - [ ] Total de candidaturas recebidas
  - [ ] Taxa de conversão (visualizações → candidaturas)
  - [ ] Gráfico de candidaturas por semana
  
- [ ] **Dashboard Admin**:
  - [ ] Total de candidatos, anunciantes, vagas ativas
  - [ ] Vagas pendentes de moderação
  - [ ] Usuários bloqueados
  - [ ] Métricas gerais do sistema

### 21. Email Transacional
- [ ] **Template de boas-vindas**: HTML responsivo
- [ ] **Template de confirmação de email**: Com link de ativação
- [ ] **Template de candidatura recebida**: Para candidato
- [ ] **Template de status de candidatura**: Selecionado/Rejeitado
- [ ] **Template de vaga expirada**: Para anunciante
- [ ] **Template de vaga aprovada/rejeitada**: Para anunciante
- [ ] **Template de recuperação de senha**: Já existe no Identity, mas customizar

### 22. Upload de Arquivos
- [ ] **Foto de perfil**: Candidate/Advertiser (max 2MB, JPG/PNG)
- [ ] **Logo da empresa**: Advertiser (max 1MB, JPG/PNG/SVG)
- [ ] **Currículo PDF**: Importar dados automaticamente (futuro)
- [ ] **Storage provider**: Interface IStorageProvider
- [ ] **Implementação local**: Para desenvolvimento
- [ ] **Implementação cloud**: Azure Blob Storage ou AWS S3 (produção)

### 23. Validação de CNPJ/CPF
- [ ] **CpfValidator**: Validar dígitos verificadores
- [ ] **CnpjValidator**: Validar dígitos verificadores
- [ ] **TaxIdValidator**: Detectar automaticamente se é CPF ou CNPJ
- [ ] **Integrar no AdvertiserValidator**

### 24. Performance e Cache
- [ ] **Cache de listagem de vagas**: 5 minutos
- [ ] **Lazy loading**: Relacionamentos carregados sob demanda
- [ ] **Paginação eficiente**: Skip/Take com índices
- [ ] **Query optimization**: Evitar N+1 queries
- [ ] **AsNoTracking**: Para queries read-only

### 25. SEO
- [ ] **Meta tags dinâmicas**: Título, descrição por vaga
- [ ] **Open Graph tags**: Para compartilhamento em redes sociais
- [ ] **Sitemap.xml**: Gerar automaticamente
- [ ] **Robots.txt**: Configurar
- [ ] **URLs amigáveis**: `/jobs/{id}-{slug}` ao invés de `/jobs/{id}`

---

## 🔵 PRIORIDADE BAIXA (Futuro/Monetização)

### 26. Sistema de Planos
- [ ] **Plan entity**: Free, Premium, Enterprise
- [ ] **Subscription entity**: Plano ativo, data início/fim, status
- [ ] **Payment entity**: Histórico de pagamentos
- [ ] **Integração gateway de pagamento**: Stripe ou MercadoPago
- [ ] **Webhook handler**: Confirmar pagamentos
- [ ] **Upgrade/Downgrade**: Mudar de plano

### 27. Boost/Highlight de Vagas
- [ ] **BoostUseCase**: Ativar boost (verificar plano)
- [ ] **HighlightUseCase**: Ativar destaque (verificar plano)
- [ ] **Boost expiration**: Desativar automaticamente
- [ ] **Highlight expiration**: Desativar automaticamente
- [ ] **Boost/Highlight avulso**: Comprar sem plano premium

### 28. Limites por Plano
- [ ] **Free**: Max 3 vagas ativas
- [ ] **Premium**: Vagas ilimitadas
- [ ] **Enterprise**: Vagas ilimitadas + features extras
- [ ] **Validar ao criar vaga**: Verificar limite do plano

### 29. API REST (Enterprise)
- [ ] **JobsController**: GET, POST, PUT, DELETE
- [ ] **ApplicationsController**: GET, POST
- [ ] **CandidatesController**: GET, POST (admin only)
- [ ] **Authentication**: JWT tokens
- [ ] **Rate limiting**: Por API key
- [ ] **Documentação**: Swagger/OpenAPI

### 30. Relatórios
- [ ] **Relatório de candidaturas**: Por vaga, período, status
- [ ] **Relatório de vagas**: Por anunciante, categoria, status
- [ ] **Relatório de candidatos**: Por skills, localização
- [ ] **Export CSV/Excel**: Download de relatórios
- [ ] **Gráficos**: Charts.js ou similar

### 31. Integrações Futuras
- [ ] **LinkedIn OAuth**: Login com LinkedIn
- [ ] **LinkedIn Import**: Importar perfil do LinkedIn
- [ ] **Indeed/Glassdoor**: Publicar vagas automaticamente
- [ ] **Google Jobs**: Schema.org markup para vagas
- [ ] **Slack/Teams**: Notificações de novas candidaturas
- [ ] **Calendar**: Agendar entrevistas

### 32. Machine Learning (Longo Prazo)
- [ ] **Recomendação de vagas**: Baseado em perfil do candidato
- [ ] **Match score**: Compatibilidade candidato-vaga
- [ ] **Salary prediction**: Sugerir salário baseado em mercado
- [ ] **Skill extraction**: Extrair skills de currículos PDF
- [ ] **Job categorization**: Categorizar vagas automaticamente

### 33. Mobile App
- [ ] **React Native** ou **Flutter**
- [ ] **Push notifications**: Candidaturas, novas vagas
- [ ] **Offline mode**: Cache local
- [ ] **Camera integration**: Upload de foto/logo
- [ ] **Biometric auth**: FaceID/TouchID

---

## 📋 CHECKLIST DE QUALIDADE

### Testes
- [ ] **Testes unitários**: Use Cases (xUnit/NUnit)
- [ ] **Testes de integração**: Repositórios + EF Core
- [ ] **Testes E2E**: Blazor (Playwright)
- [ ] **Testes de validação**: Entity validators
- [ ] **Code coverage**: Mínimo 70%

### Documentação
- [ ] **README.md**: Visão geral, como rodar
- [ ] **API documentation**: Swagger (se API REST)
- [ ] **Architecture diagram**: Clean Architecture
- [ ] **Contributing guide**: Como contribuir
- [ ] **Changelog**: Histórico de mudanças

### CI/CD
- [ ] **GitHub Actions**: Build automático
- [ ] **Testes automáticos**: Rodar em PRs
- [ ] **Deploy automático**: Produção (após testes)
- [ ] **Code quality**: SonarCloud/SonarQube
- [ ] **Security scan**: Dependabot, Snyk

### Monitoramento
- [ ] **Application Insights**: Telemetria
- [ ] **Error tracking**: Sentry, Rollbar
- [ ] **Health checks**: `/health` endpoint
- [ ] **Logging estruturado**: Serilog
- [ ] **Metrics**: Prometheus + Grafana (opcional)

---

## 🎯 METAS DE CURTO PRAZO (Próximas 2 Semanas)

### Semana 1
- [ ] Unificar target framework
- [ ] Implementar 5 repositórios principais
- [ ] Criar DTOs de Jobs
- [ ] Implementar CreateJobUseCase
- [ ] Implementar ListJobsUseCase
- [ ] Criar página `/jobs` (listagem)
- [ ] Criar página `/jobs/{id}` (detalhes)

### Semana 2
- [ ] Implementar CreateApplicationUseCase
- [ ] Implementar UpdateApplicationStatusUseCase
- [ ] Criar página `/jobs/create`
- [ ] Criar página `/apply/{jobId}`
- [ ] Criar página `/my-applications`
- [ ] Criar página `/received-applications`
- [ ] Adicionar Seed Data
- [ ] Testar fluxo completo (criar vaga → candidatar-se → mudar status)

---

## 📊 PROGRESSO ATUAL

| Fase | Status | % |
|------|--------|---|
| **Modelo de Domínio** | ✅ Completo | 100% |
| **Infraestrutura Base** | ✅ Completo | 100% |
| **Autenticação** | ✅ Completo | 100% |
| **Repositórios** | ⚠️ 1/7 implementado | 14% |
| **Use Cases** | ❌ Nenhum implementado | 0% |
| **Validações** | ❌ Nenhum validador | 0% |
| **UI Blazor** | ⚠️ 5/~20 páginas | 25% |
| **Regras de Negócio** | ❌ Nenhuma implementada | 0% |
| **Notificações** | ❌ Nenhuma | 0% |
| **Email** | ⚠️ NoOp (não envia) | 10% |
| **Seed Data** | ❌ Nenhum | 0% |
| **Testes** | ❌ Nenhum | 0% |

**Progresso Geral: ~20%**

---

*Checklist gerado em: 06/04/2026*
*Última atualização: 06/04/2026*
