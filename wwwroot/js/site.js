/* ═══════════════════════════════════════════════════════════════
   LearnHub — site.js
   ═══════════════════════════════════════════════════════════════ */

'use strict';

// ── Auto-dismiss alerts ──────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    setTimeout(() => {
        document.querySelectorAll('.alert-dismissible').forEach(el => {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(el);
            bsAlert.close();
        });
    }, 5000);
});

// ── Smooth scroll for anchor links ──────────────────────────────
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', e => {
        const target = document.querySelector(anchor.getAttribute('href'));
        if (target) {
            e.preventDefault();
            target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    });
});

// ── Courses: live search via Fetch API ───────────────────────────
const searchInput = document.querySelector('.lh-search-input');
if (searchInput) {
    let debounceTimer;
    searchInput.addEventListener('input', () => {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => {
            const query = searchInput.value.trim();
            const category = document.querySelector('input[name="category"]')?.value ?? 'All';

            if (query.length === 0) return;

            const url = `/Courses?search=${encodeURIComponent(query)}&category=${encodeURIComponent(category)}`;

            fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
                .then(res => {
                    if (res.ok) {
                        // Full page navigation for simplicity; for SPA use JSON API
                        window.location.href = url;
                    }
                })
                .catch(err => console.warn('Search error:', err));
        }, 400);
    });
}

// ── Quiz: update score ring dynamically (result page) ───────────
const scoreRing = document.querySelector('.lh-score-ring');
if (scoreRing) {
    const pctText = scoreRing.querySelector('.lh-score-num')?.textContent ?? '0%';
    const pct = parseInt(pctText);
    if (!isNaN(pct)) {
        scoreRing.style.background =
            `conic-gradient(#2563eb ${pct}%, #e2e8f0 0%)`;
    }
}

// ── Navbar: add shadow on scroll ────────────────────────────────
window.addEventListener('scroll', () => {
    const nav = document.querySelector('.lh-navbar');
    if (nav) {
        nav.style.boxShadow = window.scrollY > 10
            ? '0 4px 20px rgba(0,0,0,.3)'
            : '0 1px 0 rgba(255,255,255,.05)';
    }
});
