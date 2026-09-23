# Product authorization (Step 15)

`UserRole` is stored as an integer: User = 1, Admin = 2. New registrations
always create User accounts. The AddUserRole migration assigns User to existing
rows. Neither registration nor login accepts a role input.

JWTs contain the persisted role under `ClaimTypes.Role`, and JWT validation uses
the same role claim type with inbound claim mapping disabled. AuthResponse and
`GET /api/auth/me` expose this role without exposing password data.

Product POST and PUT use role-based authorization attributes. DELETE uses the
named `AdminOnly` policy, which requires authentication and the Admin role.
The policy centralizes a requirement; role attributes express the same simple
check directly. Product GET endpoints remain public.

Anonymous writes return 401; authenticated User writes return 403. Admin writes
reach the existing services and validation. Editing a token role invalidates its
signature and returns 401.

## Development Admin bootstrap performed

For this local database only, these accounts were registered through the normal
registration API, initially receiving User:

- `user.step15@example.com`
- `admin.step15@example.com`

Only the second account was promoted by running the following SQL through
`docker exec -i orderflow-postgres psql -U postgres -d orderflow`:

```sql
UPDATE "Users"
SET "Role" = 2
WHERE "Email" = 'admin.step15@example.com' AND "Role" = 1;
```

Exactly one row was changed. The Admin then logged in again to obtain a newly
signed Admin token. No role-changing endpoint or automatic Admin seed was added.
No account password or token is stored in source code or this document.

Role changes do not rewrite existing JWTs: clients must log in again for new
claims, and previously issued tokens retain their claims until expiration.
Tokens issued before Step 15 have no role claim and cannot authorize writes.

The committed Development JWT key is a public development placeholder, suitable
only for isolated local learning. Use a secret-store/environment signing key for
deployed environments; the application rejects the placeholder outside Development.
