# 📋 Recruiva - Análise Completa do Projeto

## 🎯 Visão Geral

**Recruiva** é uma plataforma de recrutamento e seleção gratuita e simples, construída com **.NET 9 + Blazor + SQL Server**, seguindo arquitetura **Clean/Domain-Driven Design**.

### Stack Tecnológica
- **Backend:** ASP.NET Core 9.0 (Blazor Hybrid Server + WebAssembly)
- **ORM:** Entity Framework Core 9.0.7
- **Banco de Dados:** SQL Server (LocalDB em desenvolvimento)
- **Autenticação:** ASP.NET Core Identity (Cookies + JWT)
- **Arquitetura:** Clean Architecture + DDD + CQRS
- **Internacionalização:** pt-BR + en

---

## ✅ O QUE JÁ ESTÁ IMPLEMENTADO

### 1. **Infraestrutura Base** ✅
- [x] Arquitetura Clean/DDD completa
- [x] Value Objects tipados (Id, Email, Name, Title, Url)
- [x] BaseEntity com audit trail (CreatedAt, UpdatedAt, DeletedAt, CreatedBy, UpdatedBy, DeletedBy)
- [x] Soft Delete em todas as entidades
- [x] Suporte Multi-tenant (TenantId em todas entidades)
- [x] Sistema de validação fluente (ValidationBuilder)
- [x] Pattern Result (RequestResult<T>) para evitar exceptions
- [x] Auto DI Registration (scan automático de assemblies)
- [x] Internacionalização (i18n) pt-BR/en

### 2. **Modelo de Domínio** ✅
- [x] **Candidate** - Candidatos com dados pessoais, endereço, status
- [x] **Advertiser** - Empresas/anunciantes com CNPJ/CPF, plano ativo
- [x] **Job** - Vagas com título, descrição, requisitos, salário, localização, moderação, boost
- [x] **Application** - Candidaturas com status e histórico
- [x] **Resume** - Currículos com educação, experiência, idiomas, skills
- [x] **Address** - Endereços reutilizáveis
- [x] **Notification** - Notificações do sistema
- [x] **TenantConfig** - Configuração multi-tenant
- [x] **ApplicationStatusHistory** - Histórico de mudanças de status

### 3. **Owned Types (EF Core)** ✅
- [x] JobBoost (impulso de vaga)
- [x] JobHighlight (destaque de vaga)
- [x] JobLocation (localização da vaga)
- [x] SalaryRange (faixa salarial)
- [x] ModerationInfo (moderação de vaga)
- [x] JobCounters (contadores de views/aplicações)
- [x] Education, Experience, Language (embedded no Resume)

### 4. **Banco de Dados** ✅
- [x] Migrations configuradas
- [x] 11 tabelas mapeadas
- [x] Índices estratégicos criados
- [x] Configurações de relacionamento (FKs)
- [x] ASP.NET Identity completo (Users, Roles, Claims, Logins, Tokens)

### 5. **Autenticação & Autorização** ✅
- [x] ASP.NET Core Identity configurado
- [x] Login/Register completo
- [x] 2FA (Two-Factor Authentication)
- [x] Login externo (Google, Facebook, etc.)
- [x] Recuperação de senha
- [x] Confirmação de email
- [x] JWT configurado (JwtOptions.cs)
- [x] Persistência de autenticação WASM

### 6. **UI - Blazor Components** ⚠️ Parcial
- [x] Layout principal
- [x] CRUD de Endereços (List, Create, Edit, Delete)
- [x] Páginas de Identity completas (~25 páginas)
- [ ] **FALTAM** páginas para Jobs, Candidates, Advertisers, Applications, Resumes

---

## ❌ O QUE FALTA IMPLEMENTAR

### 🔴 CRÍTICO (Core do Sistema)

#### 1. **Casos de Uso / Use Cases (CQRS)** ❌
- [ ] **CreateJobUseCase** - Criar vaga
- [ ] **UpdateJobUseCase** - Editar vaga
- [ ] **DeleteJobUseCase** - Excluir vaga (soft delete)
- [ ] **GetJobByIdUseCase** - Buscar vaga por ID
- [ ] **ListJobsUseCase** - Listar vagas (com paginação, filtros)
- [ ] **SearchJobsUseCase** - Buscar vagas (por título, categoria, localização)
- [ ] **CreateCandidateUseCase** - Registrar candidato
- [ ] **UpdateCandidateUseCase** - Atualizar candidato
- [ ] **CreateAdvertiserUseCase** - Registrar empresa
- [ ] **UpdateAdvertiserUseCase** - Atualizar empresa
- [ ] **CreateApplicationUseCase** - Candidatar-se a vaga
- [ ] **UpdateApplicationStatusUseCase** - Mudar status da candidatura
- [ ] **ListApplicationsByJobUseCase** - Listar candidaturas de uma vaga
- [ ] **ListApplicationsByCandidateUseCase** - Listar candidaturas de um candidato
- [ ] **CreateResumeUseCase** - Criar currículo
- [ ] **UpdateResumeUseCase** - Atualizar currículo
- [ ] **GetResumeByIdUseCase** - Buscar currículo

#### 2. **Repositórios** ❌
- [ ] JobRepository
- [ ] CandidateRepository
- [ ] AdvertiserRepository
- [ ] ApplicationRepository
- [ ] ResumeRepository
- [ ] NotificationRepository
- [ ] TenantConfigRepository

#### 3. **Páginas Blazor (UI)** ❌
- [ ] **Dashboard** - Painel principal
- [ ] **Listar Vagas** - Grid de vagas públicas
- [ ] **Detalhes da Vaga** - Página de visualização
- [ ] **Criar Vaga** - Formulário (Advertiser)
- [ ] **Editar Vaga** - Formulário (Advertiser)
- [ ] **Minhas Vagas** - Gestão de vagas do anunciante
- [ ] **Candidatar-se** - Formulário de aplicação
- [ ] **Minhas Candidaturas** - Lista para candidato
- [ ] **Candidaturas Recebidas** - Lista para anunciante
- [ ] **Meu Perfil (Candidato)** - Dados + Resumes
- [ ] **Meu Perfil (Empresa)** - Dados da empresa
- [ ] **Criar Currículo** - Builder de resume
- [ ] **Editar Currículo** - Editor de resume
- [ ] **Busca de Vagas** - Página de busca com filtros
- [ ] **Notificações** - Central de notificações

#### 4. **Validações de Domínio** ❌
- [ ] JobValidator (validar dados da vaga)
- [ ] CandidateValidator (validar dados do candidato)
- [ ] AdvertiserValidator (validar CNPJ/CPF, dados da empresa)
- [ ] ApplicationValidator (impedir candidatura duplicada, etc.)
- [ ] ResumeValidator (validar currículo)

---

### 🟡 IMPORTANTE (Funcionalidades Essenciais)

#### 5. **Regras de Negócio - Candidato** ❌
- [ ] **Cadastro completo**: Nome, email, telefone, data nascimento, endereço
- [ ] **Verificação de email**: Enviar link de confirmação
- [ ] **Verificação de telefone**: SMS (futuro)
- [ ] **Múltiplos currículos**: Candidato pode ter vários resumes
- [ ] **Currículo ativo**: Um resume por vez como "ativo"
- [ ] **Status da conta**: Incomplete → Active → Blocked
- [ ] **LinkedIn integration**: Importar dados do LinkedIn (futuro)

#### 6. **Regras de Negócio - Anunciante/Empresa** ❌
- [ ] **Cadastro completo**: Nome, email, telefone, CNPJ/CPF, tipo pessoa (Física/Jurídica)
- [ ] **Validação de CNPJ/CPF**: Verificar formato e dígitos verificadores
- [ ] **Verificação de email**: Confirmar email corporativo
- [ ] **Logo da empresa**: Upload de logo (URL)
- [ ] **Descrição da empresa**: Texto sobre a empresa
- [ ] **Website**: Link para site da empresa
- [ ] **Plano ativo**: Gerenciar plano de assinatura (futuro)
- [ ] **Status da conta**: Incomplete → Active → Blocked

#### 7. **Regras de Negócio - Vagas (Jobs)** ❌
- [ ] **Criar vaga**: Apenas anunciantes ativos
- [ ] **Data de expiração**: Vaga expira automaticamente
- [ ] **Status da vaga**: Active, Expired, Hidden, Rejected, Paused
- [ ] **Moderação**: Vagas novas vão para moderação (Pending → Approved/Rejected)
- [ ] **Boost/Impulso**: Pagar para destacar vaga (Featured, SuperFeatured)
- [ ] **Highlight/Destaque**: Visual especial na listagem
- [ ] **Contadores**: Views, Applications, TotalJobs, CompletedJobs
- [ ] **Localização**: On-site, Remote, Hybrid
- [ ] **Faixa salarial**: Min/Max com moeda (BRL)
- [ ] **Tags/Categoria**: Classificação da vaga
- [ ] **Limite de vagas por anunciante**: Configurar no TenantConfig (gratuito = limite baixo)

#### 8. **Regras de Negócio - Candidaturas (Applications)** ❌
- [ ] **Candidatar-se**: Candidato aplica para vaga
- [ ] **Impedir duplicata**: Mesma candidatura (CandidateId + JobId) é única
- [ ] **Status da candidatura**: Sent → Viewed → Selected/Rejected → Archived
- [ ] **Histórico de status**: Registrar todas as mudanças
- [ ] **Datas automáticas**: AppliedAt, ViewedAt, SelectedAt, RejectedAt
- [ ] **Notas do recrutador**: Campo para observações
- [ ] **Notificar candidato**: Email quando status mudar

#### 9. **Regras de Negócio - Currículos (Resumes)** ❌
- [ ] **Criar currículo**: Título, resumo, educação, experiência, idiomas, skills
- [ ] **Múltiplos currículos**: Candidato pode ter vários (ex: "Dev Júnior", "Dev Pleno")
- [ ] **Resume ativo**: Apenas um ativo por vez
- [ ] **Educação**: Instituição, curso, nível, data início/fim, status
- [ ] **Experiência**: Empresa, cargo, descrição, data início/fim
- [ ] **Idiomas**: Idioma, nível (Basic, Intermediate, Advanced, Fluent, Native)
- [ ] **Skills**: Habilidade, nível, anos de experiência
- [ ] **Importar de PDF**: Upload de currículo PDF (futuro)

#### 10. **Regras de Negócio - Notificações** ❌
- [ ] **Tipos**: NewJob, ApplicationStatus, JobExpired, System, Promotional
- [ ] **Criar notificação**: Quando candidatura muda de status
- [ ] **Marcar como lida**: IsRead + ReadAt
- [ ] **Listar não lidas**: Contador no header
- [ ] **Central de notificações**: Página dedicada

#### 11. **Regras de Negócio - Multi-Tenant** ❌
- [ ] **Isolamento por TenantId**: Cada cliente vê apenas seus dados
- [ ] **Configurações customizáveis**: DisplayName, LogoUrl, PrimaryThemeColor
- [ ] **BaseUrl**: Domínio customizado (futuro)
- [ ] **Settings JSON**: Configurações flexíveis por tenant
- [ ] **Tenant ativo**: IsActive flag

---

### 🟢 MELHORIAS (Diferenciais)

#### 12. **Busca e Filtros** ❌
- [ ] Busca por texto (título, descrição, categoria)
- [ ] Filtro por localização (cidade, estado, remoto)
- [ ] Filtro por faixa salarial
- [ ] Filtro por tipo de trabalho (On-site, Remote, Hybrid)
- [ ] Filtro por nível de experiência
- [ ] Ordenação (mais recente, salário, relevância)

#### 13. **Dashboard/Analytics** ❌
- [ ] **Para Candidatos**: Vagas recomendadas, status das candidaturas
- [ ] **Para Anunciantes**: Views das vagas, candidaturas recebidas, taxa de conversão
- [ ] **Geral**: Total de vagas ativas, total de candidatos, total de aplicações

#### 14. **Email Marketing** ❌
- [ ] **Email de boas-vindas**: Quando usuário se registra
- [ ] **Email de confirmação**: Candidatura recebida
- [ ] **Email de status**: Candidatura selecionada/rejeitada
- [ ] **Email de vaga expirada**: Notificar anunciante
- [ ] **Email promocional**: Planos premium (futuro)
- [ ] **Template de emails**: HTML responsivo

#### 15. **Upload de Arquivos** ❌
- [ ] **Foto de perfil**: Candidate/Advertiser
- [ ] **Logo da empresa**: Advertiser
- [ ] **Currículo PDF**: Upload para importar dados
- [ ] **Storage**: Local (dev) → Azure Blob Storage/AWS S3 (prod)

#### 16. **SEO e Performance** ❌
- [ ] Meta tags dinâmicas para vagas
- [ ] Sitemap.xml
- [ ] Cache de listagem de vagas
- [ ] Paginação eficiente
- [ ] Lazy loading de relacionamentos

#### 17. **Admin/Moderação** ❌
- [ ] **Dashboard admin**: Visão geral do sistema
- [ ] **Moderar vagas**: Approve/Reject com motivo
- [ ] **Gerenciar usuários**: Bloquear/desbloquear Candidate/Advertiser
- [ ] **Logs de auditoria**: Quem fez o quê e quando
- [ ] **Configurações do sistema**: TenantConfig global

---

## 💰 MODELO DE MONETIZAÇÃO (Sistema Gratuito mas Retornável)

### Estratégia Freemium

#### **Plano Gratuito (Sempre)**
- ✅ Até 3 vagas ativas por anunciante
- ✅ Candidaturas ilimitadas
- ✅ 1 currículo por candidato
- ✅ Busca básica de vagas
- ✅ Notificações por email

#### **Plano Premium (R$ 29,90/mês)**
- ⭐ Vagas ilimitadas
- ⭐ Boost de vagas (destaque na listagem)
- ⭐ SuperFeatured (topo da listagem)
- ⭐ Analytics avançado (views, conversões)
- ⭐ Múltiplos currículos por candidato
- ⭐ Filtros avançados de busca
- ⭐ Suporte prioritário

#### **Plano Enterprise (R$ 99,90/mês)**
- 🏢 Multi-tenant dedicado
- 🏢 Customização de marca (logo, cores)
- 🏢 API REST para integrações
- 🏢 Relatórios customizados
- 🏢 Gerenciamento de equipe (múltiplos recrutadores)
- 🏢 SLA garantido

### Fontes de Receita

1. **Assinaturas Premium/Enterprise** (recorrente)
2. **Boost avulso de vagas** (R$ 9,90 por 7 dias)
3. **Destaque de vagas** (R$ 4,90 por 7 dias)
4. **Vagas urgentes** (R$ 19,90 - topo absoluto por 3 dias)
5. **Acesso ao banco de currículos** (R$ 49,90/mês para recrutadores)
6. **Publicidade direcionada** (cursos, certificações - futuramente)

### Estratégia de Crescimento

- **Fase 1 (0-6 meses)**: Gratuito total, foco em adquirir usuários
- **Fase 2 (6-12 meses)**: Introduzir plano Premium com desconto early adopter
- **Fase 3 (12+ meses)**: Enterprise + API + integrações

---

## 🏗️ ARQUITETURA TÉCNICA

### Estrutura de Pastas

```
Recruiva/
├── src/
│   ├── Recruiva.Core/          # Domain Layer (net8.0)
│   │   ├── Entities/           # Entidades de domínio
│   │   ├── ValueObjects/       # Value Objects tipados
│   │   ├── Enums/              # Enumerações
│   │   ├── DTOs/               # Data Transfer Objects
│   │   ├── Interfaces/         # Contratos (Repos, UseCases, Validations)
│   │   ├── Requests/           # Request/Response patterns
│   │   ├── Validations/        # Validação fluente
│   │   ├── Exceptions/         # Exceções customizadas
│   │   ├── Converters/         # Value converters (EF)
│   │   ├── Extensions/         # Extension methods
│   │   └── Resources/          # i18n (.resx)
│   │
│   ├── Recruiva.Web/           # Web Layer (net9.0)
│   │   ├── Data/               # DbContext + Migrations + Configurations
│   │   ├── Services/           # Serviços de aplicação
│   │   ├── Repositories/       # Implementações EF
│   │   ├── Components/         # Blazor Components
│   │   │   ├── Pages/          # Páginas principais
│   │   │   ├── Account/        # Identity pages
│   │   │   └── Layout/         # Layouts
│   │   ├── Models/             # ApplicationUser
│   │   └── Configurations/     # JWT Options
│   │
│   └── Recruiva.Web.Client/    # WASM Client (net9.0)
│       ├── PersistentAuthenticationStateProvider.cs
│       └── UserInfo.cs
│
└── Recruiva.sln
```

### Padrões de Projeto

| Padrão | Status | Descrição |
|--------|--------|-----------|
| **Clean Architecture** | ✅ | Core sem dependências externas |
| **Domain-Driven Design** | ✅ | Entidades ricas, Value Objects, Aggregate Roots |
| **CQRS** | ⚠️ | Interface IUseCase existe, mas sem implementações |
| **Repository Pattern** | ⚠️ | IBaseRepository existe, mas apenas AddressRepository implementado |
| **Result Pattern** | ✅ | RequestResult<T> para evitar exceptions |
| **Fluent Validation** | ✅ | ValidationBuilder<T> configurado |
| **Multi-tenant** | ✅ | TenantId em todas entidades |
| **Soft Delete** | ✅ | IsDeleted + DeletedAt + DeletedBy |
| **Audit Trail** | ✅ | CreatedAt, UpdatedAt, CreatedBy, UpdatedBy |
| **Dependency Injection** | ✅ | Auto-registration via Assembly Scanner |

---

## 📊 STATUS ATUAL DO PROJETO

### Métricas

| Categoria | Total | Implementado | Pendente | % |
|-----------|-------|--------------|----------|---|
| **Entidades** | 11 | 11 | 0 | 100% |
| **Owned Types** | 9 | 9 | 0 | 100% |
| **Enums** | 13 | 13 | 0 | 100% |
| **Value Objects** | 5 | 5 | 0 | 100% |
| **Interfaces** | 18 | 18 | 0 | 100% |
| **Repositórios** | ~7 | 1 | 6 | 14% |
| **Use Cases** | ~17 | 0 | 17 | 0% |
| **Validadores** | ~5 | 0 | 5 | 0% |
| **Páginas Blazor** | ~20 | 5 | 15 | 25% |
| **Regras de Negócio** | ~40 | 0 | 40 | 0% |
| **DTOs** | 4 | 4 | ~30 | 12% |

### Progresso Geral: **~20% implementado**

---

## 🎯 PRIORIDADES (Roadmap Sugerido)

### **Fase 1: MVP Funcional (2-3 semanas)**
1. ✅ Repositórios (Job, Candidate, Advertiser, Application, Resume)
2. ✅ Use Cases básicos (CRUD de Jobs, Candidates, Advertisers)
3. ✅ Validações de domínio
4. ✅ Páginas Blazor essenciais:
   - Listar vagas (pública)
   - Detalhes da vaga
   - Criar vaga (Advertiser)
   - Candidatar-se
   - Minhas candidaturas (Candidate)
   - Candidaturas recebidas (Advertiser)

### **Fase 2: Funcionalidades Core (2-3 semanas)**
5. ✅ Currículos (Resume CRUD)
6. ✅ Busca e filtros de vagas
7. ✅ Notificações básicas
8. ✅ Moderação de vagas
9. ✅ Dashboard simples

### **Fase 3: Polimento (1-2 semanas)**
10. ✅ Email transacional
11. ✅ Analytics básico
12. ✅ Upload de imagens (logo, foto perfil)
13. ✅ Responsividade mobile
14. ✅ SEO básico

### **Fase 4: Monetização (Futuro)**
15. ✅ Sistema de planos
16. ✅ Boost/Highlight de vagas
17. ✅ Pagamento (Stripe/MercadoPago)
18. ✅ Analytics avançado
19. ✅ API REST

---

## 🔧 PROBLEMAS IDENTIFICADOS

### 1. **Target Framework Mismatch**
- `Recruiva.Core`: net8.0
- `Recruiva.Web`: net9.0
- `Recruiva.Web.Client`: net9.0
- **Risco:** Incompatibilidade de pacotes EF Core (Core usa 8.0.17, Web usa 9.0.7)
- **Solução:** Atualizar Core para net9.0 ou manter todos em net8.0 LTS

### 2. **Falta de Implementação de Repositórios**
- Apenas `AddressRepository` implementado
- `IBaseRepository<T>` existe mas sem implementações para entidades principais
- **Impacto:** Use Cases não podem ser implementados sem repositórios

### 3. **Sem Casos de Uso (CQRS)**
- Interface `IUseCase<TRequest, TResponse>` definida, mas zero implementações
- **Impacto:** Sem lógica de negócio orquestrada

### 4. **Sem Validações de Domínio**
- `IEntityValidator<T>`, `ValidationBuilder<T>` existem, mas sem validadores concretos
- **Risco:** Dados inválidos podem ser persistidos

### 5. **UI Incompleta**
- Apenas CRUD de Endereços implementado (página de teste?)
- Sem páginas para Jobs, Candidates, Applications, Resumes
- **Impacto:** Sistema não utilizável pelo usuário final

### 6. **Sem Seed Data**
- Nenhum dado inicial (tenants, usuários admin, etc.)
- **Impacto:** Dificuldade em testar/demo

### 7. **Email Sender NoOp**
- `IdentityNoOpEmailSender` implementado (não envia emails de verdade)
- **Risco:** Confirmação de email, recuperação de senha não funcionam em produção
- **Solução:** Integrar com SendGrid, AWS SES, ou similar

---

## 📝 RECOMENDAÇÕES

### Imediatas
1. **Unificar target framework** para net9.0 (ou manter net8.0 LTS em todos)
2. **Implementar repositórios** faltantes (Job, Candidate, Advertiser, Application, Resume)
3. **Criar Use Cases** básicos (CRUD de Jobs)
4. **Implementar validadores** de domínio
5. **Criar páginas Blazor** essenciais (listar vagas, detalhes, candidatar-se)

### Curto Prazo
6. **Configurar email real** (SendGrid gratuito até 100 emails/dia)
7. **Adicionar Seed Data** para testes
8. **Implementar busca de vagas**
9. **Sistema de notificações**
10. **Moderação de vagas**

### Médio Prazo
11. **Upload de arquivos** (logo, foto perfil)
12. **Dashboard/Analytics**
13. **Sistema de planos/monetização**
14. **Emails transacionais**
15. **SEO e performance**

### Longo Prazo
16. **API REST** para integrações
17. **Mobile app** (React Native/Flutter)
18. **Machine Learning** (recomendação de vagas)
19. **Integração LinkedIn**
20. **Importação de currículos PDF**

---

## 🚀 CONCLUSÃO

**Recruiva** tem uma **base técnica sólida** com arquitetura bem planejada, mas está apenas **20% implementado**. O modelo de domínio está completo e bem estruturado, mas falta toda a camada de aplicação (Use Cases, Repositórios, UI).

### Pontos Fortes
- ✅ Arquitetura limpa e bem estruturada
- ✅ Modelo de domínio rico e bem validado
- ✅ Multi-tenant e audit trail desde o início
- ✅ Stack moderna (.NET 9, Blazor, EF Core)

### Desafios
- ❌ Sem lógica de negócio implementada (Use Cases)
- ❌ Sem repositórios para entidades principais
- ❌ UI quase inexistente (apenas teste de Endereços)
- ❌ Sem validações de domínio
- ❌ Target framework inconsistente

### Potencial
O sistema tem **alto potencial** para ser uma plataforma de recrutamento simples, gratuita e lucrativa via modelo freemium. A base técnica permite escalar facilmente quando as funcionalidades core forem implementadas.

**Estimativa para MVP funcional:** 4-6 semanas de desenvolvimento focado.

---

*Análise gerada em: 06/04/2026*
*Versão do código: Post-Migration Initial (20250722132331)*
