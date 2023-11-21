# nS/OneWayCollider

下からジャンプするか、上から落ちてくると乗れる板

## できること

-   選択したときに出てくる外枠に接触している かつ 板の中心点より上にいる かつ 落下している ときに、BoxCollider を有効にする
-   板の中心点より下にいる ときに、BoxCollider を無効にする
-   ギズモの直方体で塗りつぶされている面を上とする (ひっくりかえしたときは ↑ と逆の挙動)
-   板を斜めにしたときの挙動は微妙 (上にいるかの判定が中心点を基準としてるので)

## つかいかた

-   [リポジトリ](https://vpm.nekomimi.studio)を[追加](vcc://vpm/addRepo?url=https://vpm.nekomimi.studio/index.json)
-   CreatorCompanion の Manage Project で nS oneWayCollider を追加 (+)
-   Unity Editor の Hierarchy あたりで右クリック → nekomimiStudio → OneWayCollider
-   Scale ツールとかで Transform の Scale とかを調整する

## ライセンス

(LICENSE.md を参照)

-   BSD 1-Clause License

### 要するに

(この「要するに」の内容はあくまで参考であり、あくまで LICENSE.md の内容のみが適用されます)

-   ソースコードを再配布するときは表記必要
-   VRChat のワールド内で使うときは表記不要
-   責任はとりません
