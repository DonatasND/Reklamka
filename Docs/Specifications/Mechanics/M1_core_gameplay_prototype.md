# M1 Core Gameplay Prototype — Game Design Specification

**Target path:** `Docs/Specifications/Mechanics/M1_core_gameplay_prototype.md`  
**Domain:** Game Design / Core Gameplay  
**Stage:** M1 Core Gameplay Prototype  
**Status:** Approved Game Design Specification  
**Parent baseline:** M0 Visual & UX runtime baseline  
**Purpose:** Source of truth for Work, Codex and QA

---

## 1. Purpose

M1 Core Gameplay Prototype должен проверить жизнеспособность базовой игровой механики Reklamka до дальнейшего развития уровней, progression, boosters, monetization и дополнительных gameplay systems.

M1 должен ответить на следующие вопросы:

1. Понимает ли игрок базовую механику за несколько секунд.
2. Понимает ли игрок причинно-следственную связь:
   `Color → Fragment Burst → Liquid → Drain / Foam → Rearrangement`.
3. Создаёт ли массовое удаление цвета ощущение большого и значимого события.
4. Влияет ли порядок использования цветов на результат.
5. Может ли игрок визуально предполагать, какой цвет сейчас выгоднее использовать.
6. Создаёт ли trapped liquid и foam recycling meaningful consequences.
7. Возникает ли ощущение `хаос → порядок`.
8. Даёт ли система основу для дальнейшего level design, difficulty, loss states, boosters и progression.

M1 является минимальным gameplay slice, а не финальной версией игры.

---

# 2. Core Gameplay Hypothesis

Основная проверяемая гипотеза M1:

> **Порядок массового удаления цветов имеет значение из-за drainability, trapped liquid, foam recycling и ограниченной во времени доступности color charges.**

Основной вопрос игрока:

> **Какой доступный цвет выгоднее превратить в жидкость именно сейчас, чтобы максимальная часть его массы получила путь к сливу и не вернулась в виде foam?**

Geometry определяет последствия выбора.

Color availability определяет цену выбора во времени.

---

# 3. Core Gameplay Loop

Основной цикл:

`Observe → Choose Color → Burst → Flow → Drain / Trap → Foam → Rearrange → Re-evaluate`

Расширенная последовательность:

1. Игрок оценивает геометрию контейнера.
2. Игрок видит 4 active color charges и 1 общий NEXT.
3. Игрок выбирает один enabled color charge.
4. Все существующие solid fragments выбранного цвета разрушаются одновременно.
5. Их масса превращается в liquid соответствующего цвета.
6. Освобождённая масса контейнера начинает перестраиваться.
7. Liquid движется вниз через доступное свободное пространство.
8. Liquid с доступом к drain покидает контейнер.
9. Liquid без пути к drain после стабилизации становится trapped.
10. Trapped liquid вспенивается.
11. Foam затвердевает в новые solid fragments того же цвета.
12. Масса окончательно перестраивается.
13. Игрок получает новое состояние поля и следующий набор доступных действий.
14. Цикл повторяется до Win или Lose.

---

# 4. Scope

M1 включает:

- tap-based выбор цвета;
- 4 active color charges;
- 1 общий NEXT;
- конечную deterministic color queue;
- повторяющиеся colors и duplicate active charges;
- массовое одновременное разрушение всех matching solid fragments;
- преобразование fragment mass в liquid;
- движение liquid вниз;
- drain;
- drained liquid;
- trapped liquid;
- foam;
- повторное превращение foam в solid fragments;
- физическую/пространственную перестройку remaining mass;
- stars;
- освобождение stars;
- движение Released Stars;
- collection через drain;
- Win;
- Lose через исчерпание доступных legal actions;
- Prototype Level 01;
- воспроизводимые QA/playtest scenarios.

---

# 5. Out of Scope

M1 не включает:

- RNG для Color Hand или queue;
- procedural level generation;
- полноценную финальную fluid simulation как отдельную продуктовую цель;
- смешивание liquid разных цветов;
- одновременную liquid activity нескольких выбранных цветов;
- viscosity как gameplay mechanic;
- surface tension как gameplay mechanic;
- liquid pressure mechanics;
- специальные типы жидкости;
- таймер уровня;
- отдельный move counter;
- lives;
- boosters;
- rewarded mechanics;
- advertisements;
- IAP;
- progression;
- currencies;
- score;
- финальную систему рейтинга уровня;
- дополнительные star-rating rules;
- locked colors;
- frozen fragments;
- bombs;
- special fragments;
- moving blockers;
- multiple drains;
- discard active charge;
- reroll Color Hand;
- manual rearrangement of active slots;
- gameplay significance порядка active slots;
- hidden liquid routes;
- one-way liquid routes;
- сложные microscopic/capillary liquid passages;
- tutorial popups как обязательную часть core prototype;
- финальный content balancing.

---

# 6. Terminology

## 6.1 Solid Fragment

Твёрдый физический элемент внутри контейнера.

Имеет gameplay color.

Solid Fragment:

- занимает пространство;
- блокирует liquid;
- взаимодействует с другими solid objects;
- не может пройти через drain;
- может содержать Contained Star;
- уничтожается при использовании matching color charge.

---

## 6.2 Original Fragment

Solid Fragment, существующий в стартовой конфигурации уровня.

---

## 6.3 Foam Fragment

Новая solid geometry, созданная из trapped liquid после foam solidification.

После создания Foam Fragment gameplay-wise является обычным Solid Fragment соответствующего цвета.

---

## 6.4 Color Charge

Одно доступное использование конкретного цвета.

Использование одного charge уничтожает **все существующие Solid Fragments этого цвета одновременно**.

Charge не соответствует одному fragment.

---

## 6.5 Active Color Hand

Набор из четырёх одновременно доступных color charge slots.

Порядок slots в M1 не имеет gameplay significance.

---

## 6.6 NEXT

Один видимый следующий color charge.

После завершения успешного turn resolution NEXT занимает освободившийся active slot.

После этого следующий элемент deterministic queue становится новым NEXT.

---

## 6.7 Liquid

Временное состояние массы уничтоженных Solid Fragments выбранного цвета.

Liquid сохраняет цвет уничтоженных fragments.

---

## 6.8 Drained Liquid

Liquid mass, пересёкшая gameplay drain region.

Drained mass навсегда удаляется из уровня.

---

## 6.9 Trapped Liquid

Значимая liquid region, которая после завершения flow/settling не имеет проходимого пути через свободное пространство к drain.

---

## 6.10 Foam

Промежуточное состояние между Trapped Liquid и новой solid geometry.

---

## 6.11 Drain

Выход из контейнера.

Drain принимает:

- Liquid;
- Released Stars.

Drain не принимает:

- Solid Fragments;
- Foam Fragments.

---

## 6.12 Contained Star

Star, находящаяся внутри конкретного Original Fragment и ещё не освобождённая.

---

## 6.13 Released Star

Star, host fragment которой уже был уничтожен.

Released Star является самостоятельным объектом контейнера.

---

## 6.14 Collected Star

Released Star, пересёкшая gameplay collection region drain.

---

## 6.15 Turn

Одна полностью разрешённая gameplay transaction от валидного tap по color charge до возврата состояния `PLAYER_READY`, `LEVEL_COMPLETE` или `LEVEL_FAILED`.

---

# 7. Color Hand

## 7.1 Structure

M1 использует:

**4 Active Color Charges + 1 общий NEXT**

Пример:

`[Blue] [Green] [Yellow] [Red]`

`NEXT: Blue`

---

## 7.2 Charge Consumption

При использовании Green:

1. расходуется ровно один Green charge;
2. все существующие Green Solid Fragments одновременно burst;
3. выполняется полный Turn Resolution;
4. если Win не достигнут, NEXT входит в освободившийся active slot;
5. следующий queue entry становится новым NEXT.

Пример:

До:

`[Blue] [Green] [Yellow] [Red]`

`NEXT: Blue`

После использования Green и завершения turn:

`[Blue] [Blue] [Yellow] [Red]`

`NEXT: Purple`

если Purple является следующим элементом заданной очереди.

---

## 7.3 Duplicate Colors

Duplicate colors разрешены.

Например:

`[Blue] [Blue] [Yellow] [Red]`

Система не должна автоматически:

- объединять duplicate charges;
- удалять duplicate;
- заменять duplicate;
- запрещать duplicate.

Duplicate может быть намеренной частью level design.

---

## 7.4 Disabled Charge

Если в контейнере не существует ни одного Solid Fragment соответствующего цвета:

> charge является disabled.

Disabled charge:

- остаётся в active hand;
- занимает active slot;
- не может быть израсходован;
- не запускает Turn;
- не вызывает Hand Update.

Если matching Solid Fragment снова существует, charge может стать enabled.

---

## 7.5 Empty Action Is Forbidden

Color charge нельзя расходовать, если для него отсутствует matching Solid Fragment.

Gameplay contract:

> валидный tap всегда должен соответствовать реальному gameplay action.

---

# 8. Deterministic Queue

Каждый M1 prototype level использует конечную вручную заданную последовательность color charges.

RNG отсутствует.

Queue должна обеспечивать воспроизводимость:

- решения;
- цены ошибки;
- foam states;
- winning sequences;
- losing sequences;
- QA scenarios.

Цвета могут повторяться через любое количество queue positions.

Queue может создавать intentional duplicate active charges.

---

# 9. Turn State Machine

Полный state machine:

```text
PLAYER_READY
    │
    │ Valid enabled Color Charge tap
    ▼
COLOR_SELECTED
    │
    ├─ Consume selected charge
    └─ Lock gameplay input
    ▼
BURST
    │
    ├─ Destroy ALL matching Solid Fragments
    └─ Contained Stars in matching fragments → Released
    ▼
LIQUID_CREATED
    ▼
FLOW_AND_MASS_SETTLING
    │
    ├─ Liquid flows
    ├─ Accessible Liquid drains
    ├─ Remaining solids rearrange
    └─ Released Stars move
    ▼
FLOW_SETTLING
    │
    └─ Allow newly opened drain paths to resolve
    ▼
LIQUID_CLASSIFICATION
    │
    ├─ Drain path exists → continue drainage
    └─ No drain path → Trapped Liquid
    ▼
FOAMING
    ▼
FOAM_SOLIDIFICATION
    │
    └─ Create same-color Solid Foam Fragments
    ▼
FINAL_SETTLING
    │
    ├─ Solids settle
    └─ Released Stars continue moving
    ▼
FINALIZE_STAR_STATES
    ▼
WIN_CHECK
    │
    ├─ Win → LEVEL_COMPLETE
    │
    └─ No Win
    ▼
HAND_UPDATE
    ▼
LOSE_CHECK
    │
    ├─ Lose → LEVEL_FAILED
    │
    └─ No Lose
    ▼
PLAYER_READY
```

---

# 10. Input Lock

Gameplay input разрешён только в:

`PLAYER_READY`

После валидного color tap:

> gameplay input немедленно блокируется.

Input остаётся заблокированным на протяжении:

- Burst;
- Liquid Creation;
- Flow;
- Drain;
- Mass Settling;
- Liquid Classification;
- Foam;
- Foam Solidification;
- Final Settling;
- Finalize Star States;
- Win Check;
- Hand Update;
- Lose Check.

Если Turn заканчивается без Win/Lose:

> input возвращается только при переходе обратно в `PLAYER_READY`.

Игрок не может активировать второй цвет во время resolution первого.

---

# 11. Fragment Lifecycle

Основной lifecycle:

`Solid Fragment → Burst → Liquid`

Если liquid drains:

`Solid → Liquid → Drained → Removed`

Если liquid becomes trapped:

`Solid → Liquid → Trapped → Foam → Solid Foam Fragment`

Foam Fragment затем может снова пройти тот же цикл:

`Foam Solid → Liquid → Drain`

или:

`Foam Solid → Liquid → Trapped → Foam Solid`

---

# 12. Burst Rule

После валидного выбора Color X:

> все Solid Fragments Color X, существующие на момент начала Turn, входят в Burst.

Это включает:

- Original Fragments;
- Foam Fragments.

Не существует правил:

- exposed-only;
- adjacency-only;
- connected-group-only;
- nearest-only.

Mass destruction выбранного цвета является фундаментальным gameplay rule M1.

---

# 13. Liquid Creation

Mass уничтоженных fragments преобразуется в Liquid того же цвета.

Пример:

`Blue Solid → Blue Liquid`

Цвет не изменяется.

В M1 один Turn содержит только один выбранный liquid color.

Следующий color action невозможен до завершения текущего Turn.

Следовательно, смешивание liquid разных selected colors отсутствует.

---

# 14. Liquid Flow

Liquid движется вниз через визуально доступное свободное пространство.

Основным препятствием является Solid Geometry.

Gameplay routes должны быть крупными и визуально читаемыми.

M1 не должен зависеть от:

- микроскопических collider gaps;
- скрытых проходов;
- капиллярного поведения;
- неочевидной surface tension.

Если пространство визуально воспринимается как закрытое, liquid не должна неожиданно проходить через него.

Если существует визуально полноценный открытый gameplay channel, liquid должна иметь возможность им воспользоваться.

---

# 15. Dynamic Drain Paths

Liquid имеет право использовать путь к drain, открывшийся в результате перестройки массы в рамках **того же Turn**.

Пример:

1. Blue bursts.
2. Blue Liquid первоначально blocked.
3. Remaining Solid Fragment падает.
4. Канал открывается.
5. Blue Liquid получает доступ к drain.
6. Blue Liquid drains.

Liquid нельзя классифицировать как Trapped до завершения соответствующей flow/settling phase.

---

# 16. Drained Rule

Liquid mass считается Drained в момент пересечения gameplay drain region.

После этого она:

- удаляется из текущего level state;
- не становится Foam;
- не возвращается в Solid Fragment;
- считается permanent reduction of mass.

---

# 17. Trapped Rule

После стабилизации flow/settling оставшаяся Liquid классифицируется по connected liquid regions.

Значимая liquid region является Trapped, если:

> она находится внутри контейнера и не имеет проходимого пути через свободное пространство к drain.

Trapped classification определяется geometry/connectivity, а не произвольным временем ожидания.

Если путь существует, Liquid должна получить возможность завершить drainage.

---

# 18. Minimum Foam Volume

Очень маленькие остаточные liquid regions могут не создавать Foam Fragment.

Они могут быть удалены как simulation residue.

Это не самостоятельная gameplay mechanic.

`Minimum Foam Volume` является implementation/balance parameter.

Его назначение:

- предотвращать микрофрагменты;
- предотвращать физический мусор;
- предотвращать бесконечные незначительные liquid residues.

Порог не должен изменять макроскопически ожидаемый игроком результат.

---

# 19. Foam Lifecycle

Для значимой Trapped Liquid:

`Trapped Liquid → Foaming → Foam Solidification → Solid Foam Fragment(s)`

Foam должен визуально сообщать:

> liquid не смогла покинуть контейнер и возвращается в solid state.

Foam не должен появляться мгновенно до того, как игрок способен увидеть trapped state.

---

# 20. Foam Mass Rule

В M1 Foam приблизительно сохраняет gameplay mass/volume соответствующей Trapped Liquid.

Обязательное увеличение массы как penalty отсутствует.

Допустимо небольшое визуальное expansion во время foaming, если оно не меняет фундаментальный gameplay outcome.

Основная цена плохого хода создаётся через:

- потраченный charge;
- сохранённую внутри контейнера массу;
- консолидацию этой массы;
- изменённую geometry;
- необходимость будущего matching charge.

---

# 21. Foam Geometry Rule

Foam создаётся:

> в области соответствующей Trapped Liquid или непосредственно около неё.

Foam не должен появляться в несвязанной случайной части контейнера.

Новая geometry:

- компактна относительно trapped region;
- приблизительно сохраняет её gameplay mass;
- может отличаться от исходной fragment geometry;
- может изменить количество отдельных fragments.

M1 не требует восстановления исходных fragment shapes.

---

# 22. Foam Color Rule

Foam всегда сохраняет исходный color:

`Blue Liquid → Blue Foam → Blue Solid Foam`

`Green Liquid → Green Foam → Green Solid Foam`

Никакого random color conversion или color mixing в M1 нет.

---

# 23. Foam Gameplay State

После Solidification Foam становится обычной Solid Geometry.

Foam Fragments:

- имеют matching gameplay color;
- блокируют liquid;
- блокируют/поддерживают solids;
- могут блокировать Released Stars;
- могут перекрывать drain;
- участвуют в gravity/collisions;
- уничтожаются следующим matching color charge.

Foam не имеет отдельной иммунности или специальной durability.

---

# 24. Foam and Drain

Foam/Solid Fragments не могут пройти через drain.

Foam может находиться над drain и физически блокировать путь.

Foam geometry не должна технически spawn'иться внутри collection/drain region таким образом, который создаёт непредусмотренный permanent collider lock.

Такое состояние является QA defect.

---

# 25. Star Lifecycle

Star имеет три gameplay states:

```text
CONTAINED
    ↓ Host Fragment Burst
RELEASED
    ↓ Cross Drain Collection Region
COLLECTED
```

Переходы необратимы.

---

# 26. Contained Star

Contained Star принадлежит конкретному Original Fragment.

При Burst host fragment:

> Star немедленно становится Released.

Foam не создаёт новых Stars.

---

# 27. Released Star

Released Star:

- существует независимо от fragments;
- подчиняется gravity;
- взаимодействует с Solid Geometry;
- может падать и перемещаться при перестройке массы;
- может оставаться blocked несколько Turns;
- не превращается в Liquid;
- не превращается в Foam;
- не может снова стать Contained.

Foam может физически заблокировать Released Star, но не меняет её lifecycle state.

---

# 28. Event-Driven Star Collection

Star Collection происходит **событийно в любой момент Turn Resolution**.

Если Released Star пересекает gameplay collection region drain:

> она немедленно переходит `Released → Collected`.

Collection не откладывается до конца Turn.

Это может произойти во время:

- Flow;
- Mass Settling;
- Foam-related rearrangement;
- Final Settling;
- любого другого физического момента текущего Turn.

Collected state является необратимым.

---

# 29. Finalize Star States

`FINALIZE_STAR_STATES` не является единственным моментом Star Collection.

Его назначение:

> зафиксировать итоговое состояние всех Stars после завершения spatial resolution непосредственно перед Win Check.

К этому моменту Stars, ранее пересёкшие collection region, уже находятся в состоянии `Collected`.

---

# 30. Blocked Released Star

Если Released Star физически лежит на Solid Fragment или заблокирована Solid Geometry:

> она остаётся Released.

Когда будущий Turn убирает препятствие:

> Star продолжает физическое движение.

Star считается Collected только после фактического пересечения drain collection region.

---

# 31. Invalid Star Trap

Предусмотренная блокировка Star removable Solid Geometry является valid gameplay state.

Состояние, где Star стала физически permanently trapped из-за collider/simulation artifact и никакое предусмотренное gameplay action не способно исправить ситуацию:

> является QA defect, а не intended Lose mechanic.

---

# 32. Win Condition

Уровень выигран, когда:

> **все Required Stars находятся в состоянии Collected.**

Для Prototype Level 01 Required Stars = 3.

Не требуется:

- уничтожать все fragments;
- drain всю массу;
- использовать всю очередь;
- иметь определённое количество оставшихся charges.

---

# 33. Win Timing and Priority

После:

`FINAL_SETTLING → FINALIZE_STAR_STATES`

выполняется:

`WIN_CHECK`

Если все Required Stars Collected:

`WIN → LEVEL_COMPLETE`

В этом случае:

- Hand Update не выполняется;
- NEXT не входит в active hand;
- новый NEXT не извлекается;
- Lose Check не выполняется;
- gameplay input не возвращается.

Win имеет приоритет над всеми resource-state последствиями завершившегося Turn.

---

# 34. Hand Update

Hand Update выполняется только если Win Check завершился без Win.

Последовательность:

1. освободившийся consumed active slot получает текущий NEXT;
2. следующий queue entry становится новым NEXT;
3. если queue entries больше нет, NEXT становится empty;
4. enabled/disabled state active charges пересчитывается по текущему Solid Fragment state.

После этого выполняется Lose Check.

---

# 35. Queue Exhaustion

Пустой deterministic queue сам по себе не означает Lose.

Если NEXT отсутствует, но active hand содержит enabled charges:

> gameplay продолжается.

---

# 36. Lose Condition

M1 Lose наступает, если одновременно выполняются все условия:

1. не все Required Stars Collected;
2. deterministic queue полностью исчерпана;
3. NEXT отсутствует;
4. среди remaining active charges нет ни одного enabled charge.

Формально:

`Stars Remain AND Queue Empty AND No Enabled Active Charge → LEVEL_FAILED`

---

# 37. No Future-Solvability Solver

M1 не пытается заранее математически определить, что уровень уже невозможно решить.

Например система не обязана автоматически объявлять Lose только потому, что нужный для оптимального решения color больше не появится.

Lose фиксируется по фактическому отсутствию legal gameplay actions после исчерпания queue.

---

# 38. Invalid Hand Lock

Следующее состояние при непустой queue является запрещённым для M1 level design:

- Required Stars remain;
- queue/NEXT ещё содержат будущие charges;
- все четыре active charges disabled;
- игрок не способен потратить charge, чтобы получить NEXT.

M1 не вводит:

- discard;
- skip;
- reroll;
- forced replacement.

Поэтому:

> каждый M1 level обязан гарантировать отсутствие такого состояния во всех предусмотренных reachable gameplay paths либо классифицировать его как invalid level configuration.

Обнаружение такого состояния при валидной предусмотренной последовательности является Level Design / QA failure.

---

# 39. Turn Completion Order

Финальный обязательный порядок:

```text
FINAL SETTLING
        ↓
FINALIZE STAR STATES
        ↓
WIN CHECK
   ├── WIN → LEVEL_COMPLETE
   │
   └── NO WIN
          ↓
      HAND UPDATE
          ↓
      LOSE CHECK
      ├── LOSE → LEVEL_FAILED
      │
      └── NO LOSE
             ↓
         PLAYER_READY
```

Этот порядок является semantic gameplay contract M1.

---

# 40. Predictability Contract

M1 является physics-assisted puzzle, а не physics-random puzzle.

Игрок должен иметь возможность приблизительно прогнозировать макрорезультат выбора по geometry.

Должны быть визуально читаемы:

- drain;
- крупные свободные channels;
- closed pockets;
- major blockers;
- support relationships;
- положение Stars;
- расположение matching colors.

Не требуется прогнозировать:

- точный угол падения fragment;
- точное вращение;
- мелкие столкновения;
- точную форму будущего Foam.

Но один и тот же стартовый state + один и тот же action не должен регулярно давать противоположные макрорезультаты исключительно из-за физического хаоса.

---

# 41. Physics vs Determinism

Детерминированными на semantic level являются:

- стартовый level configuration;
- fragment colors;
- host fragments Stars;
- initial Color Hand;
- NEXT;
- queue;
- выбранный color;
- массовый Burst всех matching solids;
- сохранение color через liquid/foam cycle;
- Star state transitions;
- Win/Lose rules.

Физика может определять:

- локальное падение;
- вращение;
- столкновения;
- settling;
- точное положение Released Star;
- локальную форму resulting mass.

Physics не должна регулярно разрушать expected dominant outcome конкретного designed action.

---

# 42. Prototype Level 01 — Purpose

Prototype Level 01 является controlled mechanics validation level.

Он не определяет глобальные правила будущих уровней.

Основная level hypothesis:

> **Green opens Blue.**

Уровень должен доказать, что:

- Green является хорошим structural opening;
- Blue-first создаёт trapped liquid и Foam;
- ошибка Blue-first recoverable;
- повторный преждевременный Blue усиливает ошибку;
- Yellow является безопасным/нейтральным выбором;
- Red связан с более поздней целью;
- несколько winning sequences существуют;
- random/небрежный порядок хуже осмысленного.

---

# 43. Prototype Level 01 — Configuration

### Colors

4:

- Blue
- Green
- Yellow
- Red

### Initial Fragments

Target:

- приблизительно 20–22 Original Fragments.

Точное число является Level 01 tuning parameter.

### Stars

3 Required Stars.

### Initial Active Hand

`[Blue] [Green] [Yellow] [Red]`

### Initial NEXT

`Blue`

### Queue

Конечная, вручную заданная, deterministic.

Точная финальная последовательность должна быть проверена после физического layout validation, но обязана поддерживать предусмотренные test scenarios этой Specification.

RNG запрещён.

---

# 44. Prototype Level 01 — Color Roles

## Green

Основной structural opener.

Green должен иметь сравнительно хороший initial drainability.

Использование Green должно:

- существенно уменьшить Green mass;
- изменить нижнюю structure;
- открыть или существенно улучшить route для Blue.

---

## Blue

Основной demonstration color для trapped liquid / foam recycling.

### Blue-first

Должен приводить к:

- массовому Blue Burst;
- заметной trapped Blue Liquid;
- Blue Foam;
- сохранению значимой Blue mass внутри контейнера;
- recoverable state.

После structural change через Green будущий Blue должен иметь существенно лучшую drainability.

---

## Yellow

Безопасный/нейтральный alternative.

Yellow-first:

- не должен быть catastrophic mistake;
- должен давать некоторый полезный progress;
- не обязан открывать основной Blue route;
- позволяет существование нескольких разумных winning sequences.

---

## Red

Более поздняя objective color.

Red связан с late Star / late structural objective.

Red-first может освободить Star раньше оптимального момента, но не должен автоматически приводить к failure.

---

# 45. Prototype Level 01 — Star Roles

## Star A — Immediate / Teaching

Должна демонстрировать:

`Burst → Release → Drain`

и сравнительно быстро становиться Collected при хорошем opening.

---

## Star B — Dependency

Связана с Blue dependency.

Blue после Green должен давать существенно более выгодное освобождение/путь Star B, чем Blue-first.

---

## Star C — Late Objective

Связана с Red или поздней структурной расчисткой.

Должна проверять поведение Released-but-blocked Star и дальнейшее открытие её пути.

---

# 46. Prototype Level 01 — Intended Scenario Classes

Точная единственная winning sequence не требуется.

Уровень должен поддерживать как минимум следующие классы поведения.

### Efficient

Примерная логика:

`Green → Blue → Yellow/Red → remaining objective`

Ожидается:

- высокая drain efficiency;
- мало Foam;
- быстрое освобождение Stars.

---

### Safe Alternative

Например:

`Yellow → Green → Blue → ...`

Ожидается:

- Win;
- немного менее прямое прохождение;
- отсутствие серьёзного наказания за Yellow-first.

---

### Recoverable Mistake

`Blue → Green → Blue → ...`

Ожидается:

1. первый Blue создаёт Foam;
2. Green изменяет geometry;
3. второй Blue перерабатывает Blue Foam;
4. значительная Blue mass теперь drains;
5. уровень остаётся решаемым.

---

### Escalated Mistake

`Blue → Blue → ...`

до meaningful geometry change.

Ожидается:

- повторная трата Blue charge;
- повторное или сохранённое Foam state;
- заметно более высокая цена ошибки;
- повышенный риск последующего Lose.

---

### Premature Objective Release

`Red → ...`

Ожидается:

- Star может стать Released;
- Star может остаться blocked;
- последующие structural changes способны освободить путь;
- Released state сохраняется между Turns.

---

# 47. Prototype Level 01 — Loss Design

Один неправильный opening не должен автоматически делать уровень безнадёжным.

Blue-first должен быть recoverable.

Repeated poor decisions могут привести к:

- расходованию необходимых charges;
- сохранению problematic Foam;
- исчерпанию legal actions;
- Lose по глобальному M1 Lose Condition.

Level 01 должен позволять наблюдать разницу между:

- efficient play;
- recoverable mistake;
- repeated misuse.

---

# 48. Level Design Contracts

Для Prototype Level 01 и последующих M1 validation levels обязательно:

1. Drain должен быть визуально читаем.
2. Major liquid routes должны быть визуально читаемы.
3. Liquid outcome не должен зависеть от невидимых microscopic gaps.
4. Один и тот же designed action должен иметь стабильный dominant outcome.
5. Blue-first должен воспроизводимо демонстрировать Foam.
6. Green-before-Blue должен воспроизводимо улучшать Blue drainability.
7. Blue-first должен оставаться recoverable.
8. Required Stars не должны попадать в unintended permanent physics trap.
9. Не должен возникать invalid hand lock при непустой queue на предусмотренных reachable paths.
10. Dead duplicate charges допустимы, если оставшиеся legal actions позволяют продолжать designed scenario.
11. Random tapping не должен быть эквивалентен по эффективности осмысленному выбору.
12. Уровень должен иметь более одного допустимого winning path, если geometry позволяет это без нарушения основной dependency.

---

# 49. Acceptance Criteria — Core System

### AC-01 — Mass Color Burst

Использование enabled color charge уничтожает все существующие Solid Fragments matching color в рамках одного массового action.

### AC-02 — Single Charge Cost

Каждый валидный Turn расходует ровно один выбранный color charge независимо от количества уничтоженных matching fragments.

### AC-03 — Input Lock

После валидного tap новый gameplay color input невозможен до завершения Turn Resolution.

### AC-04 — Liquid Color Preservation

Liquid всегда сохраняет color уничтоженных fragments.

### AC-05 — Drain Removal

Liquid, пересёкшая drain region, навсегда удаляется из level state.

### AC-06 — Dynamic Drain Path

Liquid может использовать route, открывшийся вследствие mass rearrangement в том же Turn.

### AC-07 — Trapped Classification

Значимая Liquid становится Trapped только после отсутствия проходимого drain path после соответствующей стабилизации.

### AC-08 — Foam Recycling

Trapped Liquid превращается в Foam и затем в Solid Foam Fragment(s) того же цвета.

### AC-09 — Foam Reusability

Будущий matching charge уничтожает соответствующие Foam Fragments вместе со всеми остальными matching Solid Fragments.

### AC-10 — Star Release

Burst host fragment необратимо переводит Star `Contained → Released`.

### AC-11 — Event-Driven Collection

Released Star немедленно становится Collected при пересечении drain collection region в любой момент Turn Resolution.

### AC-12 — Persistent Released Star

Blocked Released Star сохраняется между Turns и продолжает взаимодействовать с изменяющейся Solid Geometry.

### AC-13 — Four Active Charges

В M1 active hand содержит четыре charge slots.

### AC-14 — Single NEXT

Одновременно отображается ровно один общий NEXT.

### AC-15 — Deterministic Queue

Queue воспроизводима и не использует RNG.

### AC-16 — Duplicate Charges

Duplicate colors в active hand поддерживаются системой.

### AC-17 — Disabled Charge

Charge без matching Solid Fragment не расходуется и не запускает Turn.

### AC-18 — Hand Update Timing

Hand Update происходит только после `FINALIZE_STAR_STATES → WIN_CHECK`, если Win не достигнут.

### AC-19 — Win Priority

Если все Required Stars Collected после Finalize Star States, происходит `LEVEL_COMPLETE` без Hand Update и Lose Check.

### AC-20 — Lose

При remaining Required Stars, empty queue/NEXT и отсутствии enabled active charges происходит `LEVEL_FAILED`.

### AC-21 — No Automatic Full Clear Requirement

Win не требует удаления всей fragment mass.

### AC-22 — No Cross-Turn Liquid

Перед возвратом `PLAYER_READY` liquid текущего Turn должна быть разрешена в Drained или Foam/Solid state.

---

# 50. Acceptance Criteria — Prototype Level 01

### AC-L01-01

Стартовая hand:

`Blue / Green / Yellow / Red`

с общим:

`NEXT: Blue`.

### AC-L01-02

Green-first воспроизводимо улучшает Blue drainability.

### AC-L01-03

Blue-first воспроизводимо создаёт значимое количество Trapped Blue Liquid и последующий Blue Foam.

### AC-L01-04

Blue-first не является автоматически unrecoverable failure.

### AC-L01-05

После Blue-first использование Green и последующего Blue позволяет продемонстрировать Foam recycling в более выгодной geometry.

### AC-L01-06

Повторный Blue до meaningful geometry change является заметно менее эффективным использованием charge.

### AC-L01-07

Yellow-first является допустимым безопасным/нейтральным выбором.

### AC-L01-08

Red поддерживает late-objective behaviour и возможность Released-but-blocked Star.

### AC-L01-09

Все три Stars могут быть Collected при intended winning play.

### AC-L01-10

Существует более одного допустимого winning sequence.

### AC-L01-11

Repeated poor decisions способны привести к Lose через глобальное M1 exhaustion rule.

### AC-L01-12

На предусмотренных validation paths не возникает invalid hand lock при непустой queue.

---

# 51. QA Scenarios

QA должен воспроизводимо проверить минимум следующие сценарии.

## QA-01 — Green Opening

Использовать Green первым.

Проверить:

- массовый Green Burst;
- Green Liquid;
- значимый Drain;
- изменение lower geometry;
- улучшение Blue route;
- корректный Hand Update.

---

## QA-02 — Blue-First Foam

Использовать Blue первым.

Проверить:

- все Blue solids burst;
- Liquid собирается в предусмотренной trapped area;
- значимая часть не drains;
- происходит Foam;
- Foam становится Blue Solid Geometry;
- state остаётся recoverable.

---

## QA-03 — Foam Recovery

Выполнить:

`Blue → Green → Blue`

Проверить:

- первый Blue создаёт Foam;
- Green изменяет geometry;
- второй Blue уничтожает существующий Blue Foam;
- повторная Blue Liquid имеет существенно лучший drain outcome.

---

## QA-04 — Repeated Premature Blue

Выполнить:

`Blue → Blue`

без необходимого structural opener.

Проверить:

- второй Blue является legal, если Blue Foam существует;
- charge расходуется;
- результат остаётся существенно менее эффективным;
- система не создаёт случайного artificial drain route.

---

## QA-05 — Duplicate Active Charges

Создать предусмотренное состояние с двумя одинаковыми active charges.

Проверить:

- оба существуют независимо;
- использование одного не расходует второй;
- второй становится disabled, если matching solids отсутствуют;
- duplicate не удаляется автоматически.

---

## QA-06 — Disabled Charge

При отсутствии matching solids нажать соответствующий charge.

Проверить:

- Turn не начинается;
- charge не расходуется;
- NEXT не меняется.

---

## QA-07 — Released Blocked Star

Освободить Star, которая не имеет пути через Solid Geometry.

Проверить:

- Star становится Released;
- остаётся Released между Turns;
- после удаления blocker продолжает движение;
- может стать Collected позднее.

---

## QA-08 — Mid-Turn Star Collection

Создать ситуацию, где Released Star пересекает drain во время Flow/Mass Settling.

Проверить:

- `Released → Collected` происходит немедленно;
- система не ждёт Finalize Star States.

---

## QA-09 — Win Before Hand Update

Собрать последнюю Required Star в текущем Turn.

Проверить:

`FINAL SETTLING → FINALIZE STAR STATES → WIN CHECK → LEVEL_COMPLETE`

и убедиться:

- NEXT не входит в active hand;
- queue не продвигается;
- Lose Check не выполняется.

---

## QA-10 — Queue Empty but Playable

Исчерпать queue при наличии enabled active charge.

Проверить:

- Lose не происходит;
- gameplay продолжается.

---

## QA-11 — True Exhaustion Lose

Получить состояние:

- Required Stars remain;
- queue empty;
- NEXT empty;
- все remaining active charges disabled.

Проверить:

`LEVEL_FAILED`.

---

## QA-12 — Dynamic Route

Создать Turn, где Liquid первоначально blocked, но route открывается во время mass settling.

Проверить:

- Liquid не классифицируется premature Trapped;
- использует новый route;
- соответствующая mass drains.

---

## QA-13 — Foam Star Safety

Создать Foam рядом с Released Star.

Проверить:

- Star не становится Contained;
- Foam не уничтожает Star;
- Star lifecycle state сохраняется.

---

## QA-14 — No Microscopic Route Dependency

Проверить designed closed pocket.

Liquid не должна неожиданно drain через визуально незначимый collider gap.

---

## QA-15 — Repeatability

Повторить один и тот же Level 01 sequence несколько раз.

Проверить:

- dominant gameplay outcome сохраняется;
- Green opens Blue;
- Blue-first остаётся Foam case;
- физические вариации не меняют фундаментальную логику уровня.

---

# 52. Playtest Requirements

M1 playtest должен оценивать не только возможность пройти уровень.

Наблюдать:

1. Понимает ли игрок связь Color → all matching fragments.
2. Замечает ли drain как основной spatial objective.
3. Понимает ли разницу между Drained и Trapped Liquid.
4. Понимает ли, почему появилась Foam.
5. После первого Foam начинает ли игрок оценивать geometry до следующего tap.
6. Сравнивает ли доступные colors относительно drainability.
7. Использует ли NEXT для ближайшего планирования.
8. Понимает ли, что duplicate charge не обязательно следует использовать сразу.
9. Понимает ли, что Released Star может быть временно blocked.
10. Возникает ли заметная пауза/оценка перед meaningful color choices.
11. Отличается ли поведение игрока от простого последовательного/random tapping.

Ключевой qualitative success signal:

> После наблюдения хотя бы одного успешного Drain и одного понятного Foam event игрок начинает выбирать следующий color с учётом предполагаемого пути Liquid к drain.

---

# 53. M1 Failure Signals

Core hypothesis считается недостаточно подтверждённой, если playtest показывает одно или несколько состояний:

- игрок не понимает причину Foam;
- drainability невозможно предсказать визуально;
- игрок просто нажимает любые enabled colors без оценки geometry;
- random tapping почти столь же эффективен, как осмысленная последовательность;
- физика регулярно меняет expected outcome одного и того же действия;
- Foam воспринимается как случайное наказание;
- NEXT не влияет на решения;
- Color Hand становится важнее geometry;
- level решается почти одинаково независимо от порядка colors;
- ошибки слишком быстро делают level необратимо проигранным;
- invalid hand locks возникают в нормальных предусмотренных paths;
- Stars регулярно получают непредусмотренные permanent physics traps.

При обнаружении этих проблем сначала пересматриваются:

- level geometry;
- readability;
- drain routes;
- Foam feedback;
- queue configuration;
- dominant outcomes.

Новые gameplay mechanics не добавляются автоматически.

---

# 54. Semantic Gameplay Rules vs Implementation Parameters

## 54.1 Semantic Gameplay Rules — MUST NOT CHANGE without Game Design approval

Work/Codex не должны самостоятельно изменять:

- массовое уничтожение всех matching fragments;
- один charge на один mass-color action;
- 4 active charges;
- 1 общий NEXT;
- deterministic finite queue;
- возможность duplicate colors;
- отсутствие RNG;
- input lock на весь Turn;
- fragment → liquid lifecycle;
- drained mass permanent removal;
- trapped classification по отсутствию drain path;
- trapped → same-color Foam;
- Foam → same-color Solid Geometry;
- возможность повторно растворять Foam;
- отсутствие color mixing;
- Star `Contained → Released → Collected`;
- event-driven Star Collection;
- Win Condition;
- Win priority before Hand Update;
- Hand Update before Lose Check;
- Lose Condition;
- отсутствие discard/reroll;
- prohibition of invalid hand locks в designed level states;
- Prototype Level 01 dependency `Green opens Blue`;
- Blue-first recoverable Foam behaviour.

Любая техническая неоднозначность, способная изменить перечисленное поведение, должна быть возвращена в Game Design.

---

## 54.2 Implementation / Tuning Parameters — MAY BE TECHNICALLY REFINED without changing design

Work может определить или предложить технические значения для:

- конкретной продолжительности Burst animation;
- visual Flow timing;
- stability thresholds;
- velocity thresholds для определения settling;
- точного `Minimum Foam Volume`;
- конкретного способа representation Liquid;
- конкретного способа проверки drain connectivity;
- количества meshes/physics bodies, представляющих одну Foam region;
- collider configuration;
- physics damping;
- gravity tuning;
- Star collider size;
- Drain trigger dimensions;
- конкретной длительности Foam animation;
- допустимого небольшого visual Foam expansion;
- точного числа Original Fragments Level 01 в целевом диапазоне;
- точных shapes и coordinates Level 01;
- точной конечной queue Level 01 после validation;
- visual feedback enabled/disabled charges;
- internal thresholds для определения stable state.

Эти параметры могут уточняться только при условии, что они не изменяют semantic gameplay outcome.

Если технический выбор меняет:

- drainability;
- expected color order;
- recovery;
- Win/Lose;
- Foam consequence;
- Star behaviour;
- Color Hand logic;

это уже Game Design decision и должно быть возвращено на согласование.

---

# 55. Preserve

При реализации M1 необходимо сохранить:

- текущий M0 Visual & UX runtime как visual baseline;
- фундаментальный принцип массового действия выбранного цвета;
- преимущественно tap-based управление;
- визуальную читаемость контейнера;
- ощущение большого события от каждого color action;
- направление `хаос → порядок`;
- ясную причинно-следственную связь;
- простоту первого понимания.

M1 не является поводом для нового самостоятельного visual redesign.

---

# 56. Final M1 Gameplay Contract

M1 Core Gameplay Prototype определяется следующим contract:

> Игрок управляет четырьмя active color charges и видит один общий NEXT. Использование одного enabled charge расходует его и одновременно разрушает все существующие Solid Fragments выбранного цвета. Их масса превращается в Liquid того же цвета. Geometry текущего поля определяет, какая часть Liquid получает путь к drain и навсегда покидает контейнер. Значимая Liquid без пути к drain после стабилизации становится Trapped, вспенивается и возвращается как новая Solid Foam Geometry того же цвета. Будущий matching charge может снова разрушить эту Foam и повторить цикл. Массовое исчезновение fragments перестраивает remaining mass и может освобождать Stars. Освобождённая Star становится самостоятельным объектом и немедленно становится Collected при пересечении drain collection region в любой момент Turn Resolution. После Final Settling итоговые Star states фиксируются и сначала проверяется Win. Только при отсутствии Win выполняется Hand Update, затем Lose Check и возврат управления. Prototype Level 01 проверяет эту систему через dependency `Green opens Blue`, recoverable `Blue-first → Foam`, безопасный Yellow, более поздний Red objective и конечную deterministic Color Queue.

---

**End of M1 Core Gameplay Prototype Game Design Specification**

