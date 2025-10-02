# Changelog

All notable changes to HeyGPT will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added - Modern UI Overhaul (2025-01-XX)

#### Complete Visual Redesign
- **Glassmorphism Design System** - Modern translucent cards with gradient borders and elevated shadows
- **New Color Palette** - Logo-inspired colors: Navy Blue (#1a3a52), Turquoise (#7dd3c0), Coral (#e85d75)
- **Advanced Animation System** - Pulse, glow, and scale effects with BackEase easing functions
- **Modern Typography** - Clean headers with gradient underlines

#### MainWindow (800x720)
- Circular logo container with turquoise glow effect (160px)
- "HeyGPT" 36px header with "Voice-Activated AI Assistant" subtitle
- Glassmorphism status card with animated turquoise status indicator
- Modern activity log with #F0F4F8 background and 2px turquoise border
- Gradient action buttons:
  - Start: Coral gradient (#e85d75 → #ff6d85) with glow shadow
  - Stop: Turquoise gradient (#7dd3c0 → #6dc3b0)
- Bottom navigation with colored dot indicators

#### SettingsWindow
- Modern 40px "Settings" header in navy blue
- 4px gradient underline (turquoise → coral)
- Simplified subtitle: "Configure your voice-activated AI assistant"
- Increased spacing (40px margins) for better visual hierarchy

#### Theme System (EastAsianTheme.xaml)
- **New Gradients**:
  - `TraditionalGradientBrush` - Diagonal light gradient (turquoise → cream → peach)
  - `ModernGradientBrush` - Navy depth gradient
  - `AccentGradientBrush` - Turquoise horizontal
  - `CoralGradientBrush` - Coral action gradient
- **New Animations**:
  - `PulseAnimation` - Breathing effect (1→0.6 opacity, 1.2s cycle)
  - `GlowAnimation` - Shadow blur effect (10→20px, 1.5s)
  - `ScaleUp/ScaleDown` - Smooth hover effects with BackEase
- **New Card Styles**:
  - `GlassCard` - 85% white opacity, gradient border, 40px shadow, 24px corners
  - Updated `WabiSabiCard` - 95% white opacity, 30px shadow, 20px corners
  - `InfoBox` - Light turquoise background (#E8F5F4) for informational messages
  - `WarningBox` - Light coral background (#FFF5F5) for warnings
  - `InstructionBox` - Clean white background with turquoise border for guides

### Changed

#### UI Improvements
- Activity log now has minimum height (200px) to always be visible
- Activity log background changed from #F8FAFA to #F0F4F8 for better contrast
- Activity log border increased from 1px to 2px turquoise for clear definition
- Text color changed to direct #2d2d2d (charcoal) for maximum readability

#### Info Box Redesign
- Removed all opacity attributes that made text unreadable
- Porcupine AI info box now uses `InfoBox` style (was 0.2 opacity)
- Wake word list now uses `WarningBox` style (was 0.15 opacity)
- .ppn creation guide now uses `InstructionBox` style (was hardcoded semi-transparent)
- Updated text styles from `CaptionText` to `BodyText` for better readability
- Increased font sizes from 10-11px to 12px for easier reading

### Fixed
- Fixed invisible activity log due to low contrast background
- Fixed unreadable info boxes with transparent backgrounds (80-85% transparent)
- Fixed .ppn instructions visibility with proper borders and backgrounds
- Fixed text visibility issues across all settings sections

## [1.1.0] - 2024-XX-XX

### Added
- **Custom Wake Word Support** - Upload custom .ppn files from Picovoice Console
- **14 Built-in Wake Words** - jarvis, alexa, computer, hey google, hey siri, ok google, picovoice, porcupine, bumblebee, terminator, americano, blueberry, grapefruit, grasshopper
- **Comprehensive .ppn Documentation** - Step-by-step guide in Settings, README, and USER_GUIDE
- **Wake Word Dropdown** - ComboBox showing all 14 built-in options with editable text
- **Custom Wake Word Section** - Dedicated UI for uploading .ppn files with Browse/Clear buttons
- **File Validation** - Checks .ppn file exists before saving settings

### Enhanced
- **Porcupine Integration** - Industry-standard wake word detection with 97%+ accuracy
- **AccessKey Support** - Free Picovoice account integration (no credit card required)
- **Dual Engine Support** - Auto-detect Porcupine when AccessKey provided, fallback to System.Speech
- **Info Boxes** - Added clear visual feedback for wake word requirements

### Documentation
- Created comprehensive USER_GUIDE.md with .ppn creation instructions
- Updated README.md with custom wake word section and troubleshooting
- Added clear distinction between built-in (no .ppn) and custom (requires .ppn) wake words

## [1.0.0] - Initial Release

### Added
- Voice-activated ChatGPT launching with wake word detection
- Multi-monitor support with monitor configuration
- Window automation with button clicking
- OCR-based button detection
- Blue orb verification for voice mode
- System tray integration
- Auto-start with Windows
- Activity logging
- Settings persistence in %APPDATA%

---

## Legend

- **Added** - New features
- **Changed** - Changes in existing functionality
- **Deprecated** - Soon-to-be removed features
- **Removed** - Removed features
- **Fixed** - Bug fixes
- **Security** - Security improvements
