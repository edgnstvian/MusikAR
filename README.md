# MusikAR - Kelompok 1

Aplikasi Augmented Reality alat musik tradisional Indonesia (Angklung dan Sasando).
Arahkan kamera HP ke gambar marker, model 3D muncul, lalu bisa diputar (1 jari),
di-zoom (2 jari), dibuka infonya, dan diputar suaranya.

## Kebutuhan
- Unity **6000.6.9f1** (Unity 6.6), dengan modul **Android Build Support**
  (termasuk OpenJDK dan Android SDK & NDK Tools)
- HP Android (ARCore didukung), USB debugging aktif

## Cara build
1. Clone repo ini, buka foldernya lewat Unity Hub (Add -> Add project from disk).
   Import pertama bisa lama (Library dibuat ulang).
2. File -> Build Profiles -> pilih **Android** (Switch Platform kalau belum aktif).
   Scene List harus berisi `Scenes/SampleScenes` yang tercentang.
3. Colok HP, pilih HP di Run Device, klik **Build And Run**.
   Atau klik Build untuk menghasilkan APK lalu pindahkan ke HP.
4. Setelah terpasang, izinkan akses kamera.

## Setting penting (sudah diatur di project)
- IL2CPP, ARM64, Graphics API OpenGLES3 (Vulkan dihapus)
- Minimum API level 26, package name `com.kelompok1.musikar`
- Active Input Handling: Input System Package (New)

## Marker
Gambar marker ada di `Assets/Marker`. Tampilkan di layar laptop atau cetak,
lalu arahkan kamera HP ke gambar tersebut:
- `Angklung` -> model Angklung
- `Sasando` -> model Sasando

## Yang perlu dites
- [ ] Model muncul saat marker terdeteksi, hilang saat marker lepas
- [ ] 1 jari geser -> model berputar
- [ ] 2 jari cubit -> model membesar/mengecil
- [ ] Tombol **Info** menampilkan/menyembunyikan panel
- [ ] Tombol **Putar Suara** membunyikan suara (Angklung sudah ada; Sasando menyusul)

Kalau ada error saat build atau perilaku aneh, catat pesan error / foto layar
dan kirim ke pembuat project.

## Kredit
- Model 3D: (isi dari Sketchfab: judul, pembuat, lisensi)
- Suara Angklung: "g4 bandung angklung vib" oleh parking-sun, Freesound (ID 74295) - cek lisensi di halaman Freesound
- Gambar marker: (isi sumbernya)
