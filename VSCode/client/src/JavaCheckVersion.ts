export interface JavaVersionCheck {
    isJavaVersionSupported(versionString: string): boolean;
}

export class DefaultJavaVersionCheck implements JavaVersionCheck {
    public isJavaVersionSupported(versionString: string) {
        const versionPattern = new RegExp('(java|openjdk) (version)? ?"?((9|[0-9][0-9])|(1|9|[0-9][0-9])\.(1|8|[0-9][0-9]).*).*');
        return versionPattern.test(versionString);
    }
}