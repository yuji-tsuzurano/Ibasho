# データベース設計書

## 関係図

```mermaid
erDiagram
    application_user ||--o{ posts : "1:N"
    application_user ||--o{ post_likes : "1:N"
    application_user ||--o{ follows : "follower"
    application_user ||--o{ follows : "followee"
    application_user ||--o{ notifications : "target"
    application_user ||--o{ notifications : "actor"

    posts ||--o{ post_likes : "1:N"
    posts ||--o{ posts : "replies"
    posts ||--o{ notifications : "1:N"
```

---

## ER 図

```mermaid
erDiagram
    application_user {
        text id PK
        varchar user_name
        varchar normalized_user_name
        varchar email
        varchar normalized_email
        bool email_confirmed
        text password_hash
        text security_stamp
        text concurrency_stamp
        text phone_number
        bool phone_number_confirmed
        bool two_factor_enabled
        timestamptz lockout_end
        bool lockout_enabled
        int access_failed_count
        text display_name
        text bio
        text avatar_url
        text banner_url
        timestamptz created_at
        timestamptz updated_at
    }

    posts {
        bigint id PK
        text user_id FK
        bigint parent_post_id FK
        varchar content
        timestamptz created_at
        timestamptz updated_at
        bool is_deleted
    }

    post_likes {
        bigint id PK
        bigint post_id FK
        text user_id FK
        timestamptz created_at
    }

    follows {
        bigint id PK
        text follower_id FK
        text followee_id FK
        timestamptz created_at
    }

    notifications {
        bigint id PK
        text user_id FK
        text actor_user_id FK
        int type
        bigint post_id FK
        varchar message
        bool is_read
        timestamptz created_at
    }

    application_user ||--o{ posts : "1:N"
    application_user ||--o{ post_likes : "1:N"
    application_user ||--o{ follows : "follower"
    application_user ||--o{ follows : "followee"
    application_user ||--o{ notifications : "target"
    application_user ||--o{ notifications : "actor"
    posts ||--o{ post_likes : "1:N"
    posts ||--o{ posts : "replies"
    posts ||--o{ notifications : "1:N"
```

---

## テーブル項目

凡例:
- NN: 非NULL（NOT NULL）の列に ● を付与
- PK: 主キー列に ● を付与
- UK: 一意制約（Unique）を構成する列。複合一意のときは同じラベル（例: U1）を各列に付与。
- FK: 外部キー列に ● を付与（参照先は省略）

### application_user （ユーザー）

| 英語カラム名 | 日本語 | 型 (PostgreSQL) | NN | PK | UK | FK |
|--------------|--------|------------------|------|----|----|----|
| id | ユーザーID | text | ● | ● | | |
| user_name | ユーザー名 | character varying(256) |  |  |  |  |
| normalized_user_name | 正規化ユーザー名 | character varying(256) |  |  | U1 |  |
| email | メールアドレス | character varying(256) |  |  |  |  |
| normalized_email | 正規化メール | character varying(256) |  |  |  |  |
| email_confirmed | メール確認済 | boolean | ● |  |  |  |
| password_hash | パスワードハッシュ | text |  |  |  |  |
| security_stamp | セキュリティスタンプ | text |  |  |  |  |
| concurrency_stamp | 競合スタンプ | text |  |  |  |  |
| phone_number | 電話番号 | text |  |  |  |  |
| phone_number_confirmed | 電話番号確認済 | boolean | ● |  |  |  |
| two_factor_enabled | 二要素認証有効 | boolean | ● |  |  |  |
| lockout_end | ロックアウト終了 | timestamptz |  |  |  |  |
| lockout_enabled | ロックアウト有効 | boolean | ● |  |  |  |
| access_failed_count | 失敗回数 | integer | ● |  |  |  |
| display_name | 表示名 | text | ● |  |  |  |
| bio | 自己紹介 | text |  |  |  |  |
| avatar_url | アバターURL | text |  |  |  |  |
| banner_url | バナーURL | text |  |  |  |  |
| created_at | 作成日時 | timestamptz | ● |  |  |  |
| updated_at | 更新日時 | timestamptz | ● |  |  |  |

インデックス:
- UNIQUE INDEX (normalized_user_name)
- INDEX (normalized_email)
- INDEX (display_name)
- INDEX (created_at)

---

### posts（投稿）

| 英語カラム名 | 日本語 | 型 (PostgreSQL) | NN | PK | UK | FK |
|--------------|--------|------------------|------|----|----|----|
| id | 投稿ID | bigint | ● | ● |  |  |
| user_id | ユーザーID | text | ● |  |  | ● |
| content | 本文 | character varying(280) | ● |  |  |  |
| parent_post_id | 親投稿ID | bigint |  |  |  | ● |
| created_at | 作成日時 | timestamptz | ● |  |  |  |
| updated_at | 更新日時 | timestamptz | ● |  |  |  |
| is_deleted | 論理削除 | boolean | ● |  |  |  |

インデックス:
- INDEX (created_at)
- INDEX (user_id)
- INDEX (user_id, created_at)
- INDEX (parent_post_id)

---

### post_likes（投稿のいいね）

| 英語カラム名 | 日本語 | 型 (PostgreSQL) | NN | PK | UK | FK |
|--------------|--------|------------------|------|----|----|----|
| id | いいねID | bigint | ● | ● |  |  |
| post_id | 投稿ID | bigint | ● |  | U1 | ● |
| user_id | ユーザーID | text | ● |  | U1 | ● |
| created_at | 作成日時 | timestamptz | ● |  |  |  |

インデックス:
- UNIQUE INDEX (post_id, user_id)
- INDEX (post_id)
- INDEX (user_id)
- INDEX (user_id, created_at)

---

### follows（フォロー・フォロワー）

| 英語カラム名 | 日本語 | 型 (PostgreSQL) | NN | PK | UK | FK |
|--------------|--------|------------------|------|----|----|----|
| id | フォローID | bigint | ● | ● |  |  |
| follower_id | フォロワーID | text | ● |  | U1 | ● |
| followee_id | フォロー先ID | text | ● |  | U1 | ● |
| created_at | 作成日時 | timestamptz | ● |  |  |  |

インデックス:
- UNIQUE INDEX (follower_id, followee_id)
- INDEX (follower_id)
- INDEX (followee_id)

---

### notifications（通知）

| 英語カラム名 | 日本語 | 型 (PostgreSQL) | NN | PK | UK | FK |
|--------------|--------|------------------|------|----|----|----|
| id | 通知ID | bigint | ● | ● |  |  |
| user_id | 対象ユーザーID | text | ● |  |  | ● |
| actor_user_id | 行為者ユーザーID | text | ● |  |  | ● |
| type | 通知種別 | integer | ● |  |  |  |
| post_id | 関連投稿ID | bigint |  |  |  | ● |
| message | メッセージ | character varying(500) |  |  |  |  |
| is_read | 既読 | boolean | ● |  |  |  |
| created_at | 作成日時 | timestamptz | ● |  |  |  |

インデックス:
- INDEX (user_id)
- INDEX (actor_user_id)
- INDEX (post_id)
- INDEX (user_id, created_at)
- INDEX (user_id, is_read, created_at)

---

 
