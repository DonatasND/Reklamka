# M0 Static Reference Match — Structural Hard-Match Pass

**Status:** Canonical approved requirements for the final structural static pass before dynamic M0  
**Last consolidated:** 2026-08-29  
**Pass type:** Final static reference match; structural asset, material, and layout closeout  
**Definition of Done:** A reference-matched, static, completely dry runtime screen  
**Out of this pass:** Gameplay logic, input handling, fragment conversion, liquid flow, drain behavior, and all animation

This is the only canonical Visual Specification for M0. It replaces earlier requirements that ask for incremental polish of existing primitive visuals or treat gameplay mechanics as part of this pass.

## 1. Primary objective

Build the static M0 screen in Unity so it reads as close as practical to the approved reference outcome before returning to mechanics or animation.

- Judge the result against the references, not against the existing scene or generated UI.
- Treat current visual primitives, hierarchy, sprites, prefabs, and visual code as disposable when they block reference match.
- Reuse an existing visual only when it already meets this contract; otherwise replace it with an authored asset-based solution.
- Do not accept a screen simply because it is cleaner than the old prototype. It passes only when the largest visible differences in layout, silhouette, density, materials, and control treatment have been removed.

The intended review reaction is: **the static screenshot belongs to the same art system as the references; remaining differences are fine polish, not silhouette, material, density, or layout problems.**

### QA-driven priorities for this pass

1. Fully replace the remaining tubular/bone/Y/hook fragment silhouette library with short, wide, thick chunky blobs.
2. Increase pile visual mass through fewer, substantially larger pieces while keeping the pile dense but individually readable.
3. Increase selector and NEXT visual weight so the controls match the reference hierarchy rather than reading small or glass-like.
4. Preserve the successful container geometry, neutral background, and current layout improvements; only lightly refine the container rim and outlet readability.
5. Polish the top UI only after the fragment pile and lower controls meet this contract.

## 2. Canonical references

Use these images directly during implementation and screenshot review:

- `Docs/VisualReferences/gameplay_target.jpg` — canonical composition, layout, and overall screen target.
- `Docs/VisualReferences/gameplay_style_reference.jpg` — canonical soft-3D material, depth, polish, and liquid-style target.

For this static dry-state pass, use the references to match composition and material language without rendering liquid. The liquid reference establishes the future material direction only; visible liquid is prohibited in the accepted static screenshot.

## 3. Static dry-state scope

The accepted state contains only:

1. light background;
2. Back / Level / Settings top UI;
3. three empty stars;
4. one large glass-like container with an integrated bottom drain;
5. one dense dry pile of colored fragments inside the lower portion of the container;
6. one large selector capsule with four color buttons in a visually selected state;
7. one separate compact NEXT pill.

The screen starts and remains dry for the whole screenshot/review. No liquid, stream, water pool, splash, fragment pop, deformation, particles, drain flow, or automatic state change is shown.

The current approved static baseline remains in force: the new reference-relative layout, a large-fragment lower pile, enlarged selector and NEXT, neutral glass treatment, and one continuous background. This pass changes only the three remaining large visual deltas—fragment silhouettes/pile mass, fragment material, and lower-control weight—and does not authorize rollback of successful layout or visual-direction changes.

## 4. Reference-relative layout system

### Coordinate convention

All measurements are normalized against the **usable 9:16 game viewport after safe-area handling**:

- `W` = usable viewport width.
- `H` = usable viewport height.
- `x` values are measured from the left edge; `y` values from the top edge.
- Values are visual anchors, not fixed pixels. Preserve the ratios at every supported resolution.

The base screenshot target is portrait 9:16. At a different aspect ratio, retain the horizontal ratios to `W`, retain the vertical sequence to `H`, and do not introduce new empty bands between the main blocks.

### Structural layout anchors

| Element | Canonical bounds / anchor | Required treatment |
| --- | --- | --- |
| Back | center `(0.09W, 0.080H)` | Small, light outline icon; no panel. |
| Level label | center `(0.50W, 0.080H)` | `Level 1`; slightly heavier than the side icons. |
| Settings | center `(0.91W, 0.080H)` | Small, light outline icon; no panel. |
| Star row | center `(0.50W, 0.140H)` | Three empty outline stars; total group width `0.18W`. |
| Container outer shell | `x = 0.10W`, `y = 0.225H`, `width = 0.80W`, `height = 0.495H` | Dominant central vessel. |
| Container inner cavity | inset `0.025W` horizontally and `0.020H` vertically from the shell | Clips the dry pile; no visible liquid. |
| Integrated drain | centered at `x = 0.50W`; extends from container bottom to `0.742H`; width `0.160W`, height `0.035H` | Short, slightly wider integrated transparent continuation, not a separate gray part. |
| Dry fragment-pile main mass | `x = 0.135W–0.865W`, `y = 0.445H–0.695H` | Dense readable lower mass occupying 50–55% of inner container height. |
| Dry fragment-pile top contour | 2–4 limited irregular peaks may rise to `y = 0.395H–0.425H` | Raises the top contour 20–30% relative to the current QA screenshot; never forms a horizontal layer. |
| Selector capsule | `x = 0.050W`, `y = 0.745H`, `width = 0.900W`, `height = 0.128H` | Wider, taller, thick opaque off-white premium control block. |
| Button centers | `x = 0.200W, 0.400W, 0.600W, 0.800W`; `y = 0.809H` | Red, yellow, green, blue; diameter `0.180W`. |
| NEXT pill | centered at `(0.50W, 0.916H)`; width `0.350W`, height `0.066H` | 10–15% larger solid off-white secondary control, positioned closer to the selector. |

### Layout tolerances and review

- Major bounds and anchor centers must match the table within **±0.02W horizontally** and **±0.02H vertically**.
- Container-to-selector gap is `0.025H` from shell bottom to selector top; selector-to-NEXT gap is `0.010H` from selector bottom to NEXT top.
- The visual group must be compact from container through NEXT. Do not create extra whitespace in these two gaps.
- The top UI remains airy; do not solve compactness by moving the container into the stars.
- Before sign-off, compare the static runtime screenshot and references at the same 9:16 size. Scale, silhouette, spacing, and occupied areas are reviewed before small decorative details.

## 5. M0.1 — Static composition and top UI

The only approved top structure is:

**Back left → Level 1 centered → Settings right → three empty stars centered below**

- Back and Settings are thin, readable, neutral line icons.
- `Level 1` is clean, centered, and has slightly stronger visual weight and readability than the preceding screenshot, without changing the approved top composition.
- The three stars are empty, equally spaced, and have a light but readable outline stroke.
- No top panels, counters, extra labels, filled stars, progress displays, or decorative background objects.
- Background is one continuous light off-white/cool-gray field with restrained ambient depth only. It contains no visible horizontal color bands, panel seams, or runtime UI bands.

## 6. M0.2 — Container asset strategy

### Required visual form

The container is a large physical vessel, not a flat UI frame, card, panel, rounded rectangle, or thin gray outline.

- Match the reference container's dominant rounded-rectangular silhouette and overall proportions using the layout anchors above.
- Keep a straight, level upper edge, large soft corner radius, and a substantial but elegant shell.
- The container must visually hold the fragment pile and prepare a downstream route to the bottom drain.

### Dedicated container asset stack

Use a dedicated authored, reusable asset/prefab stack with separately controllable layers. A single generated rounded-rectangle primitive is not an acceptable final container.

Required layers:

1. outer glass/plastic shell and silhouette;
2. inner cavity/tint layer;
3. wall-thickness and edge-volume layer;
4. inner highlight layer;
5. outer/rim highlight layer;
6. restrained inner/bottom ambient-shadow layer;
7. separate soft grounded shadow under the vessel;
8. integrated drain shell, drain highlight, and drain inner-depth layers.

The container material is neutral white/clear translucent glass/plastic with soft-3D depth: controlled low-intensity highlights, visible wall thickness, subtle inner depth, and no photorealistic noise. It must be polished but quieter than the fragments.

- Preserve the current approved geometry, corner radius, depth, wall thickness, and neutral clear-glass result. No structural container rebuild is allowed in this pass.
- Only slightly neutralize any remaining blue tint in the rim; glass must continue to read neutral/white/clear first.
- Preserve clear-glass depth and soft white highlights. Do not reintroduce icy bloom, electric-blue rim glow, or frosted-icy treatment.

### Drain appearance in this pass

- The drain is centered, slightly wider and shorter than the prior version, visibly fused to the container's lower edge, and has slightly clearer inner-edge/connection readability.
- It uses the same translucent material family, rounding, and lighting direction as the shell.
- It is static and empty in this pass.
- Do not add liquid in, through, or below the drain.

## 7. M0.3 — Fragment asset strategy and dry pile

### Silhouette library

Perform a **full replacement** of the weak fragment-silhouette library. Create and use **8–12 visually distinct** chunky organic silhouettes across the approved families. Rotating or mirroring one source silhouette does not count as distinct.

The final library and pile use predominantly these shape families and close variants:

1. wide peanut;
2. fat kidney;
3. asymmetric pillow;
4. rounded chunky rectangle;
5. rounded triangular chunk;
6. short bent capsule;
7. wide bean;
8. irregular chunky block.

Every fragment is short, wide, thick, soft, and pillow-like. Explicitly prohibit or reduce to zero: Y-shaped pieces, branching forms, narrow necks, long tubular segments, strong hooks, worm-like silhouettes, string-like pieces, tall beans, and elongated curved forms. No final pile may be built from one generic bean, uniform capsule, flat oval, or repeated primitive shape.

### Fragment material layers

Each fragment must use a soft-3D asset/material treatment with independently controllable visual layers or their exact authored equivalent:

- base color/body;
- upper soft highlight, consistently lit from the approved scene direction;
- lower form shadow;
- edge/volume transition where needed;
- soft contact shadow where fragments overlap or touch the container base.

Add subtle local ambient occlusion/contact shadows at fragment-to-fragment contact points. The depth cue must be soft and restrained enough to preserve individual readability; it must never turn the pile into a dark tangled mass.

Required material reading:

- chunky, thick, rounded, tactile, and rubber-jelly-like;
- soft matte-gloss rather than hard plastic, sharp gloss, or wet liquid;
- visibly volumetric at screenshot scale;
- richer than a flat fill, but not photorealistic;
- clean, juicy colors: rich red, warm yellow, non-minty green, and deep expressive blue.

Use broad soft highlights and soft low-contrast specular response. Further reduce the current hard glossy-plastic appearance. Do not use narrow hard white streaks, mirror-like glare, hard plastic shine, or a strongly metallic/reflection-driven look. The visual target is soft rubber, clay, or squishy candy while preserving solid volume and contact shadows.

Fragments must visually differ from the future liquid: solids are soft semi-matte and dense; any later liquid will be glossy and translucent. No liquid is rendered now.

### Dense irregular pile composition

- Place the pile entirely inside the inner cavity and primarily inside the envelope defined in Section 4.
- Visual mass, not a fixed count, governs the pile. Use approximately **16–20** visible fragments at the target screenshot scale; fewer, substantially larger pieces than the current QA screenshot are mandatory.
- Increase average fragment scale by approximately **20–35%** relative to the current QA screenshot.
- Fill approximately **75–85%** of the pile envelope with visible fragment bodies. Minimize empty gaps while keeping irregular readable negative space rather than a regular grid.
- The pile's main mass occupies approximately **50–55%** of the inner container height, concentrated in the lower area and retaining a clearly empty upper region.
- Use at least **6** distinct silhouettes in a single static pile.
- Fragments must be densely touching and locally nested, but at least 80% of neighboring pairs retain a readable individual silhouette; no pair may look geometrically interpenetrated or tangled.
- Use light local visual contact only: normal occlusion is no greater than **8%** of local fragment width, with no crossing/intersection that hides a piece's core silhouette.
- Use rotation variance of approximately **15°–45°** across the pile and scale variance of approximately **0.85×–1.15×** around the enlarged nominal fragment size.
- Concentrate the visual mass at the lower container area. The pile's upper silhouette is uneven and organic, never a straight horizontal row or a set of even horizontal layers.
- Raise the irregular top contour approximately **20–30%** relative to the current QA screenshot using only 2–4 limited peaks in the Section 4 range. No other fragment may extend above the main-mass top at `0.445H`.
- Avoid even rows, mirrored columns, equal gaps, uniform color alternation, sparse scatter, tangled crossings, or obvious repeated arrangements.

The required result is a large, dense, packed-but-readable mass of chunky blobs closely matching the target's lower-container visual weight—not a sparse arrangement, tangled collection of tubes, or a visually noisy pile.

## 8. Selector and NEXT asset strategy

### Selector capsule

The selector is one large **thick opaque off-white** premium rounded capsule, positioned by Section 4. Its solid soft-3D material must visibly differ from the clear-glass container; it must not read as a thin glass capsule. It requires dedicated layered visual assets:

1. opaque off-white capsule base/shell;
2. restrained warm-neutral interior light gradient;
3. soft external shadow;
4. subtle internal depth/highlight;
5. four dimensional button-base assets;
6. four glossy droplet-icon assets;
7. active-state ring asset;
8. active-state soft glow asset.

- Button order is fixed: red, yellow, green, blue.
- Preserve the current selector width, height, and button dimensions captured by the Section 4 anchors. Do not begin another selector scale pass.
- Make the selector material noticeably more opaque off-white soft plastic, with restrained soft-3D depth and less glass-like transparency than the container.
- Buttons are large, circular, dimensional, and fit the numerical centers and diameter in Section 4.
- Button bases use soft plastic/rubber-like shading with gentle depth; they are neither jewel-like nor metallic. Keep the droplet itself prominent, glossy, and visibly sculpted rather than flat.
- Render one static active state for the screenshot: a clean luminous blue ring plus a smaller controlled blue bloom around the blue button. Do not use diffuse cyan haze, excessive bloom, or a foggy neon aura.
- Do not include `CURRENT COLOR` text, helper copy, or temporary labels.

### NEXT pill

NEXT requires its own dedicated assets/layers, separate from the selector:

1. pill base;
2. soft shadow;
3. restrained top/edge highlight;
4. small color-indicator asset;
5. `NEXT` text treatment.

NEXT is a solid off-white pill with no outline-style prototype look. Increase it approximately **10–15%** relative to the current QA screenshot, place it slightly closer to the selector per Section 4, and use a soft but visible shadow, slightly larger yellow swatch, and darker readable `NEXT` label. It remains secondary in hierarchy, but its material and alignment must still look finished.

## 9. Asset Validation Gate

Every M0 visual asset must pass this gate **before** scene integration, prefab assignment, or screenshot QA. An asset that fails any condition is not an accepted M0 asset, even if its silhouette or colors appear visually promising.

### Required validation for all sprite and texture assets

1. **True RGBA:** source and imported texture must contain a real alpha channel.
2. **Transparent background:** all intended background pixels must have `A = 0`; this includes canvas corners, inter-sprite sheet space, and intended empty regions.
3. **No baked checkerboard:** checkerboard pixels are RGB content, not transparency, and are prohibited anywhere in the final source asset.
4. **No white halo:** sprite edges must composite cleanly over the M0 off-white background and container without white/gray fringe, matte contamination, or premultiplied-alpha artifacts.
5. **No clipped bounds:** the visible sprite body, highlight, and soft edge must not touch or be cut by the source canvas edge. Keep sufficient transparent padding for soft highlights and shadows.
6. **Pre-integration validation:** inspect the asset at source and after import on both transparent and M0-background previews before assigning it to the scene.

For fragment sprites, validate at least the four canvas corners and multiple intended-background samples. They must return `A = 0`; visual resemblance to a checkerboard is never evidence of transparency.

### Fragment asset delivery strategy

- Prefer separate transparent PNG assets or independently controllable sprites for fragments when this improves silhouette quality, edge integrity, layout control, or QA reliability.
- A sheet is allowed only when it passes this gate and preserves independent sprite bounds, padding, and transparency.
- Do not apply a coarse color-threshold alpha cleanup to erase a fake checkerboard when that process damages highlights, soft edges, antialiasing, or the intended material response.
- Prefer regeneration/export from a source with a genuinely transparent background and real RGBA alpha.

## 10. Yellow-frame development-only visualization

The yellow Safe Area frame is confirmed as an **editor-only visualization**.

- It may remain available during development inside the editor.
- It must not be rendered into runtime, build, device capture, or final QA screenshots.
- It must not be mistaken for a scene/UI object or hidden by covering it with another visual element.
- The final QA report must state that the screenshot was captured without this editor-only visualization.

## 11. Explicitly out of Definition of Done

This pass is deliberately a static visual match. Do **not** treat the following as required for completion:

- color-button click handling;
- tapping fragments;
- selection rules or group activation;
- fragment squash, pop, breakup, removal, or particles;
- liquid spawning, flow, pooling, collision, contour following, or drain movement;
- accumulated water level or drain-out behavior;
- score, stars logic, progress, win/loss states, boosters, monetization, or extra UI.

Do not add visible placeholder mechanics merely to imply future behavior. A static selected-blue selector is presentation only, not gameplay functionality.

## 12. Fixed-resolution screenshot QA and visual acceptance criteria

The pass is accepted only when a static runtime screenshot at **720 × 1280** (9:16) passes every criterion below. After any asset repair or replacement, repeat this fixed-resolution QA before accepting the pass.

1. The screenshot is a completely dry static screen: no liquid, stream, splash, animation residue, or drain flow is visible.
2. It follows the Section 4 anchors within the stated tolerances: top UI, stars, container, dry pile, selector, and NEXT occupy the expected relative positions and sizes.
3. Compared side by side with `Docs/VisualReferences/gameplay_target.jpg`, the largest remaining structural deltas are closed: fragments read as chunky blobs rather than tubes, the pile has substantial dense visual mass, and the selector carries comparable visual weight to the target.
4. The container and background remain the approved improved base: existing geometry, clear-glass depth, neutral white/clear material, and continuous off-white/cool-gray field are preserved. Only a slight rim-tint neutralization and outlet-readability refinement are allowed.
5. The drain is transparently integrated, centered, slightly wider and shorter, and has clear enough inner-edge/connection readability that it does not look like a separate technical insert.
6. The dry pile uses approximately 16–20 large chunky pillow-like pieces, with average fragment scale 20–35% larger than the current QA screenshot. Its main mass occupies 50–55% of inner container height; 2–4 irregular peaks raise the top contour 20–30% without producing horizontal layers. The pile is densely touching with minimal gaps, retains individual readability, and leaves the upper container visibly empty.
7. The full available silhouette library contains 8–12 genuinely distinct short, broad, chunky shapes across the approved families. No Y-shape, branch, narrow neck, long tube, strong hook, worm-like piece, or excessive repetition of rotated elongated capsules remains.
8. Fragment materials read as premium soft squishy matte-gloss/rubber-clay solids with broad highlights, soft specular response, lower shading, subtle contact AO, and contact depth; they never read as hard glossy plastic or a dark tangled mass.
9. The selector preserves its current large Section 4 size but reads as noticeably more opaque off-white soft plastic than the glass container. Button bases are softer and less jewel/metal-like, droplets remain glossy, and the selected blue state is a clean luminous ring with a smaller controlled cyan halo.
10. NEXT is a separate solid off-white polished pill that is 10–15% larger than the current QA screenshot and positioned slightly closer to the selector, with soft visible shadow, slightly larger yellow swatch, and darker readable label; it has no outline-style prototype look.
11. Every visual asset integrated into the scene has passed the Asset Validation Gate and uses true RGBA transparency, free of baked checkerboards, halo, or clipped bounds.
12. The background is one continuous light off-white/cool-gray field with no horizontal runtime color bands, and the editor-only yellow Safe Area frame is absent from the runtime/device screenshot.
13. Top UI preserves the current composition and receives only light polish: `Level 1` is slightly more readable; Back, Settings, and stars remain secondary.
14. No primitive visual placeholders, debug/guide overlays, thin gray boxes, text-only symbols, or unapproved extra panels remain.
15. The screenshot uses a consistent soft-3D lighting direction and depth language across container, fragments, selector, NEXT, and top UI, placing the entire static scene in the same art system as the references.
16. Gameplay and animation are not used to compensate for missing static visual quality. The static screenshot alone must satisfy this contract.
17. Once this screenshot is accepted, every remaining reference difference is fine polish only; no further structural static redesign is required before beginning dynamic M0.

## 13. Final sign-off procedure

Before declaring this pass complete:

1. validate every newly created or changed visual asset through Section 9 before integrating it;
2. run the scene in the intended mobile viewport and capture a clean static screenshot at exactly `720 × 1280`;
3. verify the screenshot has no editor-only yellow Safe Area visualization or horizontal runtime color bands;
4. compare it side by side with `Docs/VisualReferences/gameplay_target.jpg` and `Docs/VisualReferences/gameplay_style_reference.jpg` at the same fixed resolution;
5. check the Section 4 numeric anchors and Section 12 criteria;
6. list and correct every remaining major static difference within the scope of this pass;
7. report the validation result for each replacement asset and the visual assets that replaced blocking primitives;
8. once criteria 1–17 pass, close the structural Static Reference Match and proceed to dynamic M0 planning rather than starting another static-polish loop.

Gameplay mechanics and animation are not implemented in this pass. After the accepted screenshot closes the structural Static Reference Match, the next approved stage is dynamic M0 rather than further structural static polish.


